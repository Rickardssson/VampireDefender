using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerHUD : SCR_PlayerHealth
{
   [SerializeField] private TextMeshProUGUI healthText;

   private void Start()
   {
      SetText(RemainingHealthPercentage);
   }

   private void SetText(float value)
   {
      float valuePercent = value * 100;
      healthText.SetText($"{(int)valuePercent}/100");
   }
}
