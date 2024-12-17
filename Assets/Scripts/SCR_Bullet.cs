using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class SCR_Bullet : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask PlayerLayer;
    
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    
    private Transform target;


    public void Start()
    {
        // Ignore anything in the "Player" layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
    }
    
    public void SetTarget(Transform _target)
    {
        if (_target == null)
        {
            /*Debug.LogWarning("Bullets target is null or destroyed");*/
            target = null;
            return;
        }
        
        target = _target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            /*Debug.LogWarning($"Bullet timed out and was destroyed: {gameObject.name}");*/
            Destroy(gameObject);
            return;
        }
        
        Vector2 direction = (target.position - transform.position).normalized;
        
        rb.velocity = direction * bulletSpeed;
        
        if (rb.velocity.magnitude <= 0.1f)
        {
            /*Debug.Log($"Bullet has no speed and was destroyed: {gameObject.name}");*/
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Only play SCR_Enemy_Health if the object is in the "Enemy" layer
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            SCR_EnemyHealth enemyHealth = other.gameObject.GetComponent<SCR_EnemyHealth>();
            
            if (enemyHealth != null)
            {
                Vector2 damageDirection = Vector2.zero;
                
                if (target != null)
                {
                    damageDirection = (target.position - transform.position).normalized;
                }
                
                enemyHealth.TakeDamage(
                    bulletDamage, 
                    transform.position, 
                    damageDirection);
            }
        }
        
        Debug.Log($"Bullet destroyed: {gameObject.name}");
        Destroy(gameObject);
    }
}
