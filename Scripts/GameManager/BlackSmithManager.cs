using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BlackSmithManager : MonoBehaviour,IInteract
{
    [SerializeField] private GameObject shopPanel;

    [SerializeField] private GameObject weaponBuyButton, weaponPlusButton;

    private bool isInteract = false;

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isInteract = !isInteract;
        }
    }

    private void Update()
    {
        if (isInteract)
        {
            shopPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            shopPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Interact");
            Interact();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isInteract = false;
    }

    public void OnClickWeaponBuyButton()
    {
        weaponBuyButton.SetActive(true);
        weaponPlusButton.SetActive(false);
    }

    public void OnClickWeaponPlusButton()
    {
        weaponBuyButton.SetActive(false);
        weaponPlusButton.SetActive(true);
    }
}
