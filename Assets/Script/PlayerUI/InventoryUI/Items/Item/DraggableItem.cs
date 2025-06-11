using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public ItemSlotData itemSlotData;

    [Header("Draggable Item Settings")]
    public Image iconImage;
    [HideInInspector] public Transform originalParent;
    #endregion

    public float doubleClickThreshold = 0.3f;  // 더블클릭 허용 시간
    private float lastClickTime = -1f;

    public StatusUI statusUI;
    public EquipmentUI equipmentUI;

    public GameObject E;

    #region [Unity LifeCycle]
    private void OnEnable()
    {
        itemSlotData = GetComponent<ItemSlotData>();
        iconImage = GetComponent<Image>();
        Debug.Log("itemSlotData and iconImage components initialized.");
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin drag");
        originalParent = transform.parent;
        transform.SetParent(UIManager.Instance.playerUI.inventoryUI.transform);
        transform.SetAsLastSibling();
        SelectedItemUI.Instance.GetItemSlotData(itemSlotData);
        iconImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("dragging");
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        if (!transform.parent.CompareTag("ItemSlot") && !transform.parent.CompareTag("PlayerUI")) transform.SetParent(originalParent);
        transform.SetAsFirstSibling();
        iconImage.raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 대상 확인 (굳이 이미지 오브젝트에만 적용하고 싶다면)
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            // 더블클릭 감지
            Debug.Log("더블클릭!");
            OnDoubleClick();
        }

        lastClickTime = Time.time;
    }

    void OnDoubleClick()
    {
        // 여기에 원하는 함수 호출
        Debug.Log("더블클릭 함수 실행");
        ItemType itemType = itemSlotData.itemData.itemType;
        if (itemType == ItemType.Equipable)
        {
            if (itemSlotData.itemData is EquipableItem equipableItem)//형변환
            {

                if (equipableItem.equipableType == EquipableType.Weapon)
                {
                    if (equipmentUI.WeaponItemSlotData == equipableItem)//같은장비를 클릭한거라면 해제
                    {
                        equipmentUI.WeaponItemSlotData = null;
                        E.SetActive(false);
                    }
                    else
                    {
                        equipmentUI.WeaponItemSlotData = equipableItem;//다른장비거나 없다면 장착
                        E.SetActive(true);
                    }
                }

                                if (equipableItem.equipableType == EquipableType.Armor)
                {
                    if (equipmentUI.ArmorItemSlotData == equipableItem)//같은장비를 클릭한거라면 해제
                    {
                        equipmentUI.ArmorItemSlotData = null;
                        E.SetActive(false);
                    }
                    else
                    {
                        equipmentUI.ArmorItemSlotData = equipableItem;//다른장비거나 없다면 장착
                        E.SetActive(true);
                    }
                }


                statusUI.ChangeStat(
                    equipableItem.wDamage,
                    equipableItem.wDefense,
                    equipableItem.wHP,
                    equipableItem.wCrit
                );
            }
        }
    }
    #endregion
}