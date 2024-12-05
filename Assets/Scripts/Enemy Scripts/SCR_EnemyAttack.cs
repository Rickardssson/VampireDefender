using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class SCR_EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float attackRate = 1f;

    private float timeUntilAttack;
    
    private void Update()
    {
        timeUntilAttack += Time.deltaTime;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timeUntilAttack >= 1f/attackRate && collision.gameObject.GetComponent<TopDownMovement>())
        {
            var playerHealth = collision.gameObject.GetComponent<SCR_PlayerHealth>();
            Debug.Log("I attacked you foul vampire!");
            timeUntilAttack = 0f;
            
            SCR_KnockbackFeedBack knockbackComponent = 
                collision.gameObject.GetComponent<SCR_KnockbackFeedBack>();
            if (knockbackComponent != null)
            {
                knockbackComponent.PlayFeedback(gameObject);
            }
            
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
