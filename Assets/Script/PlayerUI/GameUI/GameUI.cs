using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public GameObject buildUI;
    public GameObject inventoryUI;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    private void Start()
    {
        
    }
    #endregion


    // ========================== //
    //     [Public Methods]
    // ========================== //
    #region [Public Methods]
    // Used for buttons -------------------
    public void OnInvnetoryButton()
    {
        PlayerMediator.Instance.controller.InventoryToggle();
    }

    // ------------------------------------

    #endregion
}
