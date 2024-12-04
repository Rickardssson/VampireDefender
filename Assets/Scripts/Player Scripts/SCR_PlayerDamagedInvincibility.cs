using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SCR_PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField] private float invincibilityTime;
    private SCR_InvincibilityController invincibilityController;

    public void Awake()
    {
        invincibilityController = GetComponent<SCR_InvincibilityController>();
    }

    public void StartInvincibility()
    {
        invincibilityController.StartInvincibility(invincibilityTime);
    }
}
