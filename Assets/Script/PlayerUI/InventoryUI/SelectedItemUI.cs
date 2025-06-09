using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// itemSlotData의 메서드를 사용하는 클래스
/// </summary>
public class SelectedItemUI : MonoSingleton<SelectedItemUI>
{
    private ItemSlotData itemSlotData;

    void Start()// 초기에는 선택된 아이템 UI를 비활성화
    {
        gameObject.SetActive(false);
    }

/*
    public void ActivePopUp()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false); // 이미 활성화되어 있다면 비활성화
        }
        else
        {
            gameObject.SetActive(true); // 비활성화되어 있다면 활성화
        }
    }
*/
    public void GetItemSlotData(ItemSlotData itemSlotInfo)//지정한 아이템슬롯 값받기
    {
        itemSlotData = itemSlotInfo; 
    }

    public void AddItemButton()
    {
        itemSlotData.AddItemQuantity(1);
}
    public void OnThrowItemButton() //수량 1감소
    {
        itemSlotData.SubtractItemQuantity(1); // 아이템 수량을 1 감소시킨다.
        //UIManager.Instance.playerUI.inventoryUI.ThrowItem(itemSlotData.itemData, 1); // 아이템을 월드에 생성한다
    }

    public void OnThrowAllItmeButton() //아이템 제거
    {
        itemSlotData.ClearSlot(); // 아이템 수량을 0으로 설정하여 아이템을 제거한다.
        //UIManager.Instance.playerUI.inventoryUI.ThrowItem(itemSlotData.itemData, itemSlotData.ItemQuantity);
    }

    public void OnUseItemButton() //아이템 사용
    {
        
        if (itemSlotData.itemData == null || itemSlotData.ItemQuantity <= 0)
        {
            Debug.LogWarning("사용할 수 있는 아이템이 없습니다.");
            return; // 아이템이 없거나 수량이 0인 경우 사용하지 않음
        }

        ItemType itemType = itemSlotData.itemData.itemType;
        string itemName = itemSlotData.itemData.itemName;

        if (itemType == ItemType.Consumable)
        {
            if (itemName == "apple") PlayerMediator.Instance.status.hunger.Add(20f);
            if (itemName == "appleSoup")
            {
                PlayerMediator.Instance.status.hunger.Add(20f);
                PlayerMediator.Instance.status.health.Add(10f);
                PlayerMediator.Instance.status.thirst.Add(20f);
            }
            if (itemName == "water") PlayerMediator.Instance.status.thirst.Add(20f);

            itemSlotData.SubtractItemQuantity(1); // 아이템 수량을 1 감소시킨다.
        }

    }

    public void OnExitSelectedItemButton()// 선택된 아이템 UI를 비활성화
    {
        gameObject.SetActive(false);
    }
}
