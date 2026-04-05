using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public GameObject Inventory;

    public void Start()
    {
        Inventory.gameObject.SetActive(false);
    }

    public void Switch()
    {
        if (Inventory.activeSelf == false) Inventory.SetActive(true);
        else Inventory.SetActive(false);
        }
    }

