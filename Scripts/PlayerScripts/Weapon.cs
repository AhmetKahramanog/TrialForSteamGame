using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string _name;

    public float damage;
    private Collider collider;
    private float attackCount = 0;

    public Weapon(string name)
    {
        _name = name;
    }

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<BaseEnemy>(out BaseEnemy Enemy))
        {
            attackCount++;
            if (attackCount == 1)
            {
                if (Enemy.IsBlocked)
                {
                    Enemy.TakeDamage(damage - 10);
                }
                else
                {
                    Enemy.TakeDamage(damage);
                }
            }
        }
    }


    public void OpenCollision()
    {
        collider.isTrigger = true;
    }

    public void CloseCollision()
    {
        collider.isTrigger = false;
        attackCount = 0;
    }

}
