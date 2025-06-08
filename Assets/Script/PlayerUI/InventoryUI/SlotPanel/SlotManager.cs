using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 만들다 만듯함
/// </summary>
public class SlotManager : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    public GameObject[] slots; 
    public int slotCounts = 20; // 슬롯 개수
    public Sprite buttonPressedImage;
    public Sprite buttonNotpressedImage;
    public Transform slotPanel;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Awake()
    {
        slotPanel = gameObject.transform;
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    #endregion


    // ========================== //
    //     [Private Methods]
    // ========================== //
    #region [Private Methods]
    private void AddMoreSlots()
    {
        // AddMoreSLots : 인벤토리내 슬롯이 부족할 시, 슬롯 개수를 늘려주는 메소드
    }
    #endregion
}
