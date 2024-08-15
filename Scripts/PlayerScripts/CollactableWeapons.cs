using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollactableWeapons : MonoBehaviour
{
    [SerializeField] private Sprite icon;
    public Sprite GetIcon()
    {
        return icon;
    }
}
