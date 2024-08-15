using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponBuyManager : MonoBehaviour
{
    [SerializeField] private int weaponPrice;

    [Inject] private GoldManager goldManager;

    public void OnClickBuyButton()
    {
        goldManager.GoldKeeper(weaponPrice * -1);
    }
}
