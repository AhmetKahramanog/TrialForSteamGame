using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<GameObject> weapons;
    [SerializeField] private TextMeshProUGUI goldCountText;
    
    public float maxCapacity;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddWeapon(GameObject item)
    {
        if (IsInventoryFull()) { return; }
        weapons.Add(item);
        item.SetActive(true);
        
    }

    private void OnEnable()
    {
        GoldManager.OnGoldKeeper += GoldManager_OnGoldKeeper;
    }

    private void OnDisable()
    {
        GoldManager.OnGoldKeeper -= GoldManager_OnGoldKeeper;
    }

    private void GoldManager_OnGoldKeeper(float gold)
    {
        goldCountText.text = gold.ToString();
    }

    public bool IsInventoryFull()
    {
        if (weapons.Count > maxCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
