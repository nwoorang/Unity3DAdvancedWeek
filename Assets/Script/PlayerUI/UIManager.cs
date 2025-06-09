using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // ========================== //
    //     [Inspector Window]
    // ========================== //
    #region [Inspector Window]
    [Header("Connected Components")]
    public PlayerUI playerUI;

    public GameObject inventoryUI;
    #endregion


    // ========================== //
    //     [Unity LifeCycle]
    // ========================== //
    #region [Unity LifeCycle]
    protected override void Awake()
    {
        base.Awake();
        playerUI = GetComponentInChildren<PlayerUI>();
    }
    #endregion
}
