using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        else return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ResourcePickUp"))
        {
            rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                StartCoroutine(SmoothStop(rb));
            }
        }
    }
    
    private IEnumerator SmoothStop(Rigidbody2D bloodRb)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Vector2 initialVelocity = bloodRb.velocity;

        while (elapsed < duration)
        {
            if (bloodRb == null) yield break;
            elapsed += Time.deltaTime;
            
            bloodRb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, elapsed / duration);
            yield return null;
        }
        
        bloodRb.velocity = Vector2.zero;
    }
    
}
