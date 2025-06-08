using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

// ---------------------------------------------- //

public class BasicItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject itemPrefab;

    [Header("Stacking")]
    public bool isStackable;
    public int maxStackAmount;
}
