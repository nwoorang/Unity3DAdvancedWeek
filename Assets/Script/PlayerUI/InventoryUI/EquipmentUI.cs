using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    // 2가지의 장비 저장공간
    //장비가 있는상태에서 다른 장비가 들어오면 스위치
    //같은 장비를 클릭했다면 해제

    public EquipableItem WeaponItemSlotData;
    public EquipableItem ArmorItemSlotData;
}
