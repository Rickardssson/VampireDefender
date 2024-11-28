using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRE : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TopDownMovement>())
        {
            var playerHealth = collision.gameObject.GetComponent<SCR_Player_Health>();
            
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
