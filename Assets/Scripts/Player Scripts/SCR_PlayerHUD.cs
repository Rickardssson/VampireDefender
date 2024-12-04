using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerHUD : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI healthText;
   private GameObject player = GameObject.FindGameObjectWithTag("Player");
   private float playerRemHealth;

   private void Start()
   {
      playerRemHealth = player.GetComponent<SCR_PlayerHealth>().RemainingHealthPercentage;
   }

   private void Update()
   {
      
      SetText(playerRemHealth);
   }

   private void SetText(float value)
   {
      float valuePercent = value * 100;
      healthText.SetText($"{(int)valuePercent}/100");
   }
}
