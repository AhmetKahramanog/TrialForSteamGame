using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Image> itemImages;

    private void OnEnable()
    {
        PlayerInteractObjects.OnUpdadetItemIcon += Inventory_OnUpdadetItemIcon;
    }

    private void OnDisable()
    {
        PlayerInteractObjects.OnUpdadetItemIcon -= Inventory_OnUpdadetItemIcon;
    }

    private void Inventory_OnUpdadetItemIcon()
    {
        var inventoryItem = Inventory.Instance.weapons;
        try
        {
            for (int i = 0; i < itemImages.Count; i++)
            {
                if (i < itemImages.Count)
                {
                    itemImages[i].sprite = inventoryItem[i].GetComponent<CollactableWeapons>().GetIcon();
                    itemImages[i].enabled = true;
                }
                else
                {
                    itemImages[i].enabled = false;
                }
            }
        }
        catch (ArgumentOutOfRangeException e)
        {
            //Debug.LogError(e.Message);
        }
    }
}
