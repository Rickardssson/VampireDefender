using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Invincibility_Controller : MonoBehaviour
{
    private SCR_Player_Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<SCR_Player_Health>();
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
