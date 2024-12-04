using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownMovement>())
        {
            var playerHealth = collision.gameObject.GetComponent<SCR_PlayerHealth>();
            Debug.Log("I attacked you foul vampire!");
            
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
