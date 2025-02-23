using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class SCR_PlayerHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    private CinemachineImpulseSource impulseSource;
    
    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    
    public bool IsInvincible { get; set; }

    public float RemainingHealthPercentage
    {
        get
        {
            return currentHealth / maxHealth;
        }
    }

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth == 0)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }
        
        // ScreenShake
        SCR_CameraShakeManager.Instance.CameraShake(impulseSource);
        
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
    }

    public void addHealth(float amountToAdd)
    {
        if (currentHealth == maxHealth)
        {
            return;
        }
        
        currentHealth += amountToAdd;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

}
