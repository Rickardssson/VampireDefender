using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_CoolDownTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image imageCoolDown;

    [SerializeField] private GameObject areaAttack;
    private SCR_AreaOfEffectAttack coolDownScript;
    private bool isCoolDown = false;
    private float coolDownTimer;
    private float coolDownTime;

    private void Start()
    {
       imageCoolDown.fillAmount = 0f;
       coolDownScript = areaAttack.GetComponent<SCR_AreaOfEffectAttack>();
       coolDownTime = coolDownScript.coolDown;
    }

    private void FixedUpdate()
    {
        coolDownTimer = coolDownScript._coolDown;
        ApplyCoolDown();
    }

    private void ApplyCoolDown()
    {
        if (coolDownTimer < 0.0f)
        {
            imageCoolDown.fillAmount = 0f;
        }
        else
        {
            imageCoolDown.fillAmount = coolDownTimer / coolDownTime;
        }
    }
    
}
