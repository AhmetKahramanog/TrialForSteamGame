using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager
{
    private float gold;

    public static event Action<float> OnGoldKeeper;

    public void GoldKeeper(float amount)
    {
        gold += amount;
        OnGoldKeeper?.Invoke(gold);
    }
}
