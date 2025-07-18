using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipableType
{
    
Weapon,
   
Armor,
    
Shoes,
}
[CreateAssetMenu(fileName = "Equipable ItemData", menuName = "New ItemData/New EquipableItem")]
public class EquipableItem : BasicItemData
{
    [Header("Prefabs")]
    public GameObject equipCameraPrefab;   // 1인칭 시점용 프리팹
    public GameObject playerHandPrefab;    // 3인칭 캐릭터 손 프리팹


    public EquipableType equipableType;
    [Header("Stat")]
    public int wDamage;
    public int wGather;

    public int wDefense;
    public int wHP;
    public int wCrit;
}
