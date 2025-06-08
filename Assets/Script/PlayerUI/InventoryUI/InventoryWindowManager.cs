using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리
/// </summary>
public class InventoryWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private GameObject CraftingPanel;
    [SerializeField] private Button craftingButton;
    [SerializeField] private RectTransform TransformAlone;
    [SerializeField] private RectTransform inventoryPaneltTransform;
    [SerializeField] private RectTransform craftingPanelRectTransform;

    public void InventoryTransformInit()
    {
        inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
    }

    public void OnInventoryButton()
    {
        if (CraftingPanel.activeInHierarchy)
        {
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
    }

    public void OnCraftingButton()
    {
        if (CraftingPanel.activeInHierarchy)
        {
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = TransformAlone.anchoredPosition;
        }
        else if (!CraftingPanel.activeInHierarchy)
        {
            inventoryPanel.GetComponent<RectTransform>().anchoredPosition = inventoryPaneltTransform.anchoredPosition;
        }
    }
}
