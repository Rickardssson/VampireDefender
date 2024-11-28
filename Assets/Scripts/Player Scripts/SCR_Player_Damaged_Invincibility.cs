using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SCR_Player_Damaged_Invincibility : MonoBehaviour
{
    [SerializeField] private float invincibilityTime;
    private SCR_Invincibility_Controller invincibilityController;

    public void Awake()
    {
        invincibilityController = GetComponent<SCR_Invincibility_Controller>();
    }

    public void StartInvincibility()
    {
        invincibilityController.StartInvincibility(invincibilityTime);
    }
}
