using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCREnemyHealth : MonoBehaviour
{
    [Header("Attributes")] [SerializeField]
    private int hitPoints = 6;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
