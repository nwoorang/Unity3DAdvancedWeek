using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    Health,
    Hunger,
    SpeedBoost,
}

// ---------------------------------------------- //

[Serializable]
public class ConsumableItemData
{
    public ConsumableType type;
    public float value;
}

// ---------------------------------------------- //

[CreateAssetMenu(fileName = "Consumable ItemData", menuName = "New ItemData/New ConsumableItem")]
public class ConsumableItem : BasicItemData
{
    [Header("Consumable Info")]
    public ConsumableItemData[] consumables;
}
