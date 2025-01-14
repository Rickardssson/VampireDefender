using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerHUD : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI healthText, bloodText, baseHealthText;
   [SerializeField] private SCR_BuildingManager buildingManager;
   [SerializeField] private Image healthBar;
   [SerializeField] private Image baseHealthBar;
   private GameObject player;
   private GameObject _base;
   private float playerRemHealth;
   private float playerRemBlood;
   private float baseRemHealth;
   
   private SCR_PlayerHealth _playerHealth;
   private SCR_BaseHealth _baseHealth;

   private void Start()
   {
      player = GameObject.FindGameObjectWithTag("Player");
      _playerHealth = player.GetComponent<SCR_PlayerHealth>();
      _base = GameObject.FindGameObjectWithTag("Base");
      _baseHealth = _base.GetComponent<SCR_BaseHealth>();
      
   }

   private void Update()
   {
      playerRemHealth = _playerHealth.RemainingHealthPercentage; 
      playerRemBlood =  buildingManager.currency;
      baseRemHealth = _baseHealth.RemainingHealthPercentageBase;
      
      SetTextHealth(playerRemHealth);
      SetTextBlood(playerRemBlood);
      SetTextBaseHealth(baseRemHealth);
      UpdateHealthBar(playerRemHealth);
      UpdateBaseBar(baseRemHealth);
      
   }

   private void SetTextHealth(float value)
   {
      float valuePercent = value * 100;
      healthText.SetText($"Vitality: {(int)valuePercent}/100");
   }
   private void SetTextBlood(float value)
   {
      bloodText.SetText($": {(int)value}");
   }
   
   private void SetTextBaseHealth(float value)
   {
      float valuePercent = value * 100;
      baseHealthText.SetText($"Base health: {(int)valuePercent}/100");
   }
   
   private void UpdateHealthBar(float remainingHP)
   {
      if (healthBar != null)
      {
         healthBar.fillAmount = remainingHP;
      }
   }
   
   private void UpdateBaseBar(float remainingBaseHP)
   {
      if (baseHealthBar != null)
      {
         baseHealthBar.fillAmount = remainingBaseHP;
      }
   }
}
