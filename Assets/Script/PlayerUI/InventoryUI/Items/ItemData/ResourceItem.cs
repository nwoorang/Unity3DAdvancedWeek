using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Adobe,
}

[CreateAssetMenu(fileName = "Resource ItemData", menuName = "New ItemData/New ResourceItem")]
public class ResourceItemData : BasicItemData
{
    [Header("ResourceItemData Info")]
    public ResourceItemData[] resources;
}
