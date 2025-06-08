using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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


    // =========================== //
    //     [Unity LifeCycle]
    // =========================== //
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
    #endregion
}