using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Kind",menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public float attackDamage;
    public AnimatorOverrideController animatorOverrideController;

}
