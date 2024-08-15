using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MarketManager : MonoBehaviour
{
    public float goldInventory;

    [Inject] private GoldManager goldManager;

    [SerializeField] private GameObject showInteractTextUI;

    private bool isInteract = false;

    private void Update()
    {
        if (isInteract && Input.GetKeyDown(KeyCode.E))
        {
            MarketProcess();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            showInteractTextUI.SetActive(true);
            isInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            showInteractTextUI.SetActive(false);
            isInteract = false;
        }
    }

    private void MarketProcess()
    {
        goldManager.GoldKeeper(goldInventory);
        showInteractTextUI.SetActive(false);
        goldInventory = 0;
    }
}
