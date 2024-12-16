using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SCR_EnemyHealth : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private int hitPoints = 6;
    [Header("References")]
    [SerializeField] private GameObject coinToDrop;

    public delegate void OnDamaged(Vector2 attackPosition, Vector2 attackDirection);
    public event OnDamaged DamageEvent;

    private void Start()
    {
        /*Debug.Log($"Registering enemy {gameObject.name} with DamageEvent.");*/
        SCR_BloodParticles.Instance.RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        if (SCR_BloodParticles.Instance != null)
        {
            SCR_BloodParticles.Instance.UnregisterEnemy(this);
        }
    }

    public void TakeDamage(int dmg, Vector2 attackPosition, Vector2 attackDirection)
    {
        hitPoints -= dmg;
        
        DamageEvent?.Invoke(transform.position, attackDirection);
        
        if (hitPoints <= 0)
        {
            Instantiate(coinToDrop, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
