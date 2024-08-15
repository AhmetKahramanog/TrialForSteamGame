using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject interactObjectsText;

    private void Start()
    {
        interactObjectsText.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            interactObjectsText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            interactObjectsText.gameObject.SetActive(false);
        }
    }


}
