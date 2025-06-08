using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ItemSlotData : MonoBehaviour, IDropHandler
{
    #region [Inspector Window]
    [TextArea(6, 5)]
    public string ScriptINFO = "01) ItemSlotData게임 오브젝트에 붙어 해당 슬롯에 담긴 아이템의 정보 (itemData 스크립트)를 인벤토리 내에서 담고 있는 역할 (holder).\n" +
                               "02) 단순히 아이템 Scriptable Object 데이터만 들고있는 역할을 하는 itemData 스크립트와는 달리 해당 아이템의 인벤토리 내 수량을 관리해주는 역할도 해준다.";
    [Space(10)]

    [Header("Item Slot Settings")]
    public int itemSlotIndex;

    private int _itemQuantity;
    public int ItemQuantity
    {
        get { return _itemQuantity; }
        set
        {
            // 아이템 수량(itemQuantity)이 해당 아이템 데이터의 최대 스택 수량(maxStackAmount)보다 작거나 같을 시에만 새로운 수량을 설정한다.
            if (value >=1 && value <= itemData.maxStackAmount)
            {
                _itemQuantity = value;
                // 수량이 변경되었을 때 아이템 수량 텍스트를 업데이트한다.
                SetItemQuantityText();
            }

            if (value < 1) _itemQuantity = 0;
        }
    }

    public Sprite emptyIcon;
    public Image itemIcon;
    public GameObject itemIconBackground; // 아이템 아이콘의 배경 이미지 (슬롯 배경 이미지로 사용될 수 있음)
    public TextMeshProUGUI itemQuantityText;
    public GameObject itemQuantityBG;
    public BasicItemData itemData;
    public Transform itemDropPosition; // 아이템 드롭 위치 (드래그 앤 드롭 시 사용)

    [Header("Selected Item")]
    public RectTransform itemSlotRectTransform; // 해당 슬롯의 RectTransform (UI 위치 조정용)
    private BasicItemData selectedItem;
    #endregion



    #region [Unity LifeCycle]
    private void Start()
    {
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>();
    }
    #endregion



    #region [Public Methods]

    public void GetItemData(BasicItemData newItemData)
    {
        /// • (Completed) GetItemData : 해당 슬롯에 담긴 아이템의 데이터를 넣어준다.
        itemData = newItemData;
    }

    public void SetItemIcon(BasicItemData newItemData)
    {
        /// • (Completed) SetItemIcon : 해당 슬롯에 담긴 아이템의 아이콘을 설정해준다.

        itemIcon.sprite = newItemData.itemIcon;
        itemIcon.color = Color.white; // 아이콘 색상을 흰색으로 설정
    }

    public void AddItemQuantity(int amount)
    {
        /// • (Completed) AddItemQuantity : 해당 슬롯에 담긴 아이템의 수량을 증가시켜준다.
        ItemQuantity += amount;

        if (ItemQuantity >= itemData.maxStackAmount)
        {
            // 만약 아이템 수량이 최대 스택 수량을 초과하면 최대 스택 수량으로 설정한다.
            ItemQuantity = itemData.maxStackAmount;
            // 수량이 최대 스택 수량을 초과하면 슬롯이 다 찼다는 의미로 텍스트 색상을 빨간색으로 변경
            itemQuantityText.color = Color.red; 
        }
    }

    public void SubtractItemQuantity(int amount)
    {
        /// • (Completed) SubtractItemQuantity : 해당 슬롯에 담긴 아이템의 수량을 감소시켜준다.

        //
        if (ItemQuantity <= 0) return;

        if (ItemQuantity != itemData.maxStackAmount)
        {
            // 수량이 최대 스택 수량을 초과하지 않으면 텍스트 색상을 원래대로 변경
            itemQuantityText.color = new Color(0.356f, 0.149f, 0.149f);;
        }

        ItemQuantity -= amount;

        // 만약 아이템 수량이 0 이하가 되면 해당 슬롯을 비운다.
        if (ItemQuantity <= 0)
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {        
        /// • (Completed) ClearSlot : 해당 슬롯을 비운다.

        itemData = null;             // 아이템 데이터를 비운다.
        itemIcon.sprite = emptyIcon; // 아이콘을 empty sprite로 설정한다.
        ItemQuantity = 0;            // 수량을 0으로 설정한다.
        itemQuantityText.text = string.Empty; // 아이템 수량 텍스트를 비운다.
        itemQuantityBG.SetActive(false); // 아이템 수량 배경을 비활성화한다.
    }

    // Used for buttons and events -----------------------
    public void OnClickSlotButton()
    {
        /// • (WIP) 해당 슬롯을 클릭했을 때 작게 해당 아이템으로 취할 수 있는 액션을 보여주는 팝업을 보여주는 기능구현

        // 선택된 아이템 팝업 UI가 꺼져있다면 활성화한다.
        if (!SelectedItemUI.Instance.gameObject.activeSelf)
        {
            SelectedItemUI.Instance.gameObject.SetActive(true);
        }

        // 현재 슬롯의 정보를 SelectedItemUI에 전달한다.
        SelectedItemUI.Instance.GetItemSlotData(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        /// • (Completed) OnDrop : 드래그 앤 드롭 이벤트가 발생했을 때 호출되는 메서드
        /// Drag and Drop으로 옮기는 게임오브젝트는 ItemSlot 밑의 자식 오브젝트인 'ItemSlotData' 오브젝트이다.

        // eventData.pointerDrag에서 드래그중인 아이템 정보를 가져와준다.
        GameObject newItem = eventData.pointerDrag;

        // newItem의 DraggableItem 컴포넌트를 가져온다. 만약 newItem의 ItemSlotData를 접근해할 경우, 해당 DraggableItem의 컴포넌트를 통해 접근 가능하다 (newDr
        DraggableItem newDraggableItem = newItem.GetComponent<DraggableItem>();
        BasicItemData newBasicItemData = newDraggableItem.itemSlotData.itemData;

        // 현재 슬롯에 있는 ItemSlotData의 부모 transform을 임시로 저장한다.
        Transform tempParentSaved = transform.parent;

        // 슬롯 교체 : newItem을 현재 슬롯의 부모의 자식으로 설정하고, newItem의 부모를 현재 itemSlotData의 부모로 설정한다.
        if (transform.parent.CompareTag("ItemSlot"))
        {
            newItem.transform.SetParent(tempParentSaved);
            transform.SetParent(newDraggableItem.originalParent);
        }
    }
    // ---------------------------------------------------
    #endregion

    #region [Private Methods]
    private void SetItemQuantityText()
    {
        /// • (Completed) SetItemQuantityText : 해당 슬롯에 담긴 아이템의 수량을 표시해준다.
        /// ItemQuantity 프로퍼티내에서의 로직을 통해 수량이 변경될 때마다 호출된다.

        // 아이템이 스택 가능한 아이템이고, 수량이 1보다 크면 수량을 표시해준다.
        if (itemData.isStackable && _itemQuantity >0)
        {
            itemQuantityText.text = _itemQuantity.ToString();
            itemIconBackground.SetActive(true); // 아이템 아이콘 배경을 활성화한다.
        }
        else
        {
            itemQuantityText.text = string.Empty;
            itemIconBackground.SetActive(false); // 아이템 아이콘 배경을 비활성화한다.
        }
    }
    #endregion
}
