using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_InvincibilityController : MonoBehaviour
{
    private SCR_PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<SCR_PlayerHealth>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        StartCoroutine(InvincibilityStart(invincibilityDuration));
    }

    private IEnumerator InvincibilityStart(float invincibilityDuration)
    {
        playerHealth.IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        playerHealth.IsInvincible = false;
    }
}
