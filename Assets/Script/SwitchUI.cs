using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUI : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Status;

    public GameObject Back;
    public void OnInventory()
    {

        gameObject.SetActive(false);
        Inventory.SetActive(true);
        Back.SetActive(true);
    }
    public void OnStatus()
    {
        gameObject.SetActive(false);
        Status.SetActive(true);
        Back.SetActive(true);
    }

    public void OnGoMain()
    {
        Inventory.SetActive(false);
        Status.SetActive(false);
        Back.SetActive(false);
        gameObject.SetActive(true);
    }
}
