using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/* [ClassINFO : InventoryUI]
   @ Description : InventoryUI 게임 오브젝트에 붙어 인벤토리 UI를 관리 및 인벤토리가 수행하는 역할 맡아준다.
   @ Attached at : UI -> PlayerUI -> InventoryUI
   @ Methods :
*/

public class InventoryUI : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    private UIManager uiManager;
    private PlayerUI playerUI;
    private InventoryWindowManager inventoryWindowMangaer;

    [Header("Inventory Settings")]
    public ItemSlotData[] itemSlotDatas;
    public GameObject inventoryUI;
    public Transform slotPanel;
    public Transform itemDropPosition;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
        playerUI = uiManager.GetComponentInChildren<PlayerUI>();
        inventoryWindowMangaer = GetComponent<InventoryWindowManager>();
        InventoryInit();
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    public bool IsInventoryOpen()
    {
        /// • (Completed) IsInventoryOpen : 인벤토리가 열려있는지 확인
        return inventoryUI.activeSelf;
    }

    public void ToggleInventory()
    {
        /// • (Completed) ToggleInventory : 인벤토리 UI의 현재 상태를 확인하고, 인벤토리를 열거나 닫는다

        if (IsInventoryOpen())
        {
            inventoryUI.SetActive(false);
        }
        else
        {
            inventoryUI.SetActive(true);
            inventoryWindowMangaer.InventoryTransformInit(); // 인벤토리 패널 초기화
        }

        PlayerMediator.Instance.controller.uiOn = true;
      //  PlayerMediator.Instance.controller.ToggleCursor(); //커서 잠금,열기
    }

    public void AddItem(BasicItemData newItemData)
    {
        /// • (Completed) AddItem : 아이템을 인벤토리에 추가한다

        // 추가하려는 아이템이 이미 인벤토리에 있는 경우
        if (GetItemSlotToAdd(newItemData, out ItemSlotData existingItemSlotData))
        {
            // 해당 아이템이 있는 슬롯을 찾아 ItemSlotData에 담긴 수량을 1 증가시켜준다.
            existingItemSlotData.ItemQuantity++;
        }

        // 추가하려는 아이템이 인벤토리에 없는 경우 (새 아이템 추가)
        else AddItemToEmptySlot(newItemData);
    }

    public void SubtractOrDeletItem(BasicItemData newItemData, int quantity)
    {
        /// • (Completed) DeleteItem : 아이템의 수량을 줄이거나 인벤토리에서 제거한다
      
        if (GetItemSlotToSubtract(newItemData, out ItemSlotData existingItemSlotData))
        {
            existingItemSlotData.SubtractItemQuantity(quantity);
            return;
        }
        else
        {
            Debug.LogWarning("해당 아이템이 인벤토리에 없습니다.");
            return;
        }
    }

    public bool SearchItem(BasicItemData newItemData, out ItemSlotData itemSlotdata)
    {
        /// • (Completed) SearchItem : 인벤토리 내에서 해당 아이템이 있는지 확인하고, 있다면 true를 반환한다
        /// out값으로 반환되는 itemSlotdata를 통해 해당 아이템의 개수를 확인할 수 있다. (itemSlotdata.ItemQuantity)

        // 인벤토리 내 슬롯을 검사하여 해당 아이템이 있는지 확인
        for (int i = 0; i < itemSlotDatas.Length; i++)
        {
            if (itemSlotDatas[i].itemData == newItemData)
            {
                // 해당 아이템이 있는 슬롯을 찾았을 때, 해당 아이템을 담고 있는 itemSlotData는 out으로, return으로는 true를 반환
                itemSlotdata = itemSlotDatas[i];
                return true;
            }
        }

        // 인벤토리 내 해당 아이템이 없다면, itemSlotdata는 null로 out반환하고 false를 리턴반환
        itemSlotdata = null;
        return false;
    }

    public void ThrowItem(BasicItemData newItemData, int howMany)
    {
        /// • (Completed) ThrowItem : 인벤토리에서 아이템을 제거하고, 해당 아이템의 오브젝트를 월드에 생성한다

        for (int i = 0; i < howMany; i++)
        {
            if (newItemData == null)
            {
                Debug.LogWarning("아이템 데이터가 null입니다. 아이템을 드랍할 수 없습니다.");
                return;
            }

            Instantiate(newItemData.itemPrefab, itemDropPosition.position, Quaternion.identity);
        }
    }

    // Used for buttons -------------------
    public void OnInventory(InputAction.CallbackContext context)
    {
        /// • (Completed) OnInventory : 인벤토리 UI를 열거나 닫는 행위를 인풋시스템의 단축키를 통해서 작동할수 있도록 연결해주는 메서드
        if (context.phase == InputActionPhase.Started)
        {
            ToggleInventory();
        }
    }
    // ------------------------------------
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void InventoryInit()
    {
        /// • (Completed) InventoryInit : 인벤토리 UI 초기화


        // 각 슬롯에 있는 ItemSlotData를 가져와 ItemSlotDatas 배열에 저장
        itemSlotDatas = new ItemSlotData[slotPanel.childCount];

        for (int i = 0; i < slotPanel.childCount; i++)
        {
            // 각 슬롯의 ItemSlotData 컴포넌트를 가져와 itemSlotDatas 배열에 저장
            itemSlotDatas[i] = slotPanel.GetChild(i).GetComponentInChildren<ItemSlotData>();

            // 각 ItemSlotData의 인덱스를 설정
            itemSlotDatas[i].itemSlotIndex = i;
        }
    }

    private bool GetItemSlotToSubtract(BasicItemData newItemData, out ItemSlotData existingItemSlotData)
    {
        /// • (Completed) GetItemSlotToSubTract : 인벤토리 내에서 해당 아이템 데이터가 담긴 슬롯이 있는지 확인하고, 있다면 해당 슬롯의 ItemSlotData를 out을 통해 반환한다. (Subtract용)

        existingItemSlotData = itemSlotDatas
                               .Where(slotData => slotData.itemData == newItemData) // 해당 아이템 데이터가 있는 슬롯만 필터링
                               .OrderBy(slotData => slotData.ItemQuantity)          // 해당 아이템 데이터가 있는 슬롯을 수량이 적은 순서로 정렬
                               .FirstOrDefault();                                   // 가장 작은 ItemQuantity를 가지고 있는 첫 번째 슬롯만 반환

        if (existingItemSlotData == null) return false;
        return true;
    }

    // Used in AddItem() Method -------------------
    private bool GetItemSlotToAdd(BasicItemData newItemData, out ItemSlotData existingItemSlotData)
    {
        /// • (Completed) GetItemSlotToAdd : 인벤토리 내에서 해당 아이템 데이터가 담긴 슬롯이 있는지 확인하고, 있다면 해당 슬롯의 ItemSlotData를 out을 통해 반환한다. (Add용)

        existingItemSlotData = null;

        foreach (var slotData in itemSlotDatas)
        {
            // 만약 해당 슬롯 데이터 안에 담겨있는 itemData(basicItemData)가 newItemData와 같다면 == (습득한 아이템이 존재한다는 뜻)
            // 그리고 해당 슬롯의 아이템 수량이 최대 스택 수량보다 작다면 (== 아이템이 스택 가능하다면)
            if (slotData.itemData == newItemData && slotData.ItemQuantity != slotData.itemData.maxStackAmount)
            {
                // 해당 슬롯 데이터와 true값을 반환한다.
                existingItemSlotData = slotData;
                return true;
            }
        }

        return false;
    }

    private void AddItemToEmptySlot(BasicItemData newItemData)
    {
        /// • (Completed) AddItemToEmptySlot : 인벤토리 내에서 비어있는 슬롯을 찾아 새 아이템을 추가한다.

        // 비어있는 새로운 아이템 슬롯을 인벤토리 내에서 찾는다.
        ItemSlotData emptySlot = GetEmptySlot();

        // (!예외처리!) 만약 비어있는 슬롯이 없다면, 아이템을 추가할 수 없다는 메시지를 출력하고 아이템을 드랍한다.
        if (emptySlot == null)
        {
            Debug.LogWarning("인벤토리에 빈 슬롯이 없습니다. 아이템을 추가할 수 없습니다.");
            ThrowItem(newItemData, 1);

            return;
        }

        // 새로운 빈 ItemSlot에 새로 추가된 아이템의
        // - ItemData를 넘겨주고,
        // - 아이템 슬롯내 아이템 아이콘을 설정해주고,
        // - 수량을 1로 설정해준다. 
        emptySlot.GetItemData(newItemData);
        emptySlot.SetItemIcon(newItemData);
        emptySlot.AddItemQuantity(1); 
    }

    private ItemSlotData GetEmptySlot()
    {
        /// • (Completed) GetEmptySlot : 인벤토리 내에서 비어있는 슬롯 (== iItemData가 비어있는 슬롯)을 찾아 반환한다.

        foreach (var slotData in itemSlotDatas)
        {
            // 해당 슬롯 데이터 안에 담겨있는 itemData가 없고, itemQuantity가 0인 경우 
            if (slotData.itemData == null && slotData.ItemQuantity == 0)
            {
                // 슬롯 데이터를 반환
                return slotData;
            }
        }

        return null;
    }
    // --------------------------------------------
    #endregion

}
