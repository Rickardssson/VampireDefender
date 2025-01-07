using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Magnet : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 targetDirection;

    private void Update()
    {
        player = gameObject.transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "ResourcePickUp")
        {
            targetDirection = (collision.gameObject.transform.position - player.position).normalized;
            rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * -4.5f;
            Debug.Log("Magnetism");
        }
    }
    
}
