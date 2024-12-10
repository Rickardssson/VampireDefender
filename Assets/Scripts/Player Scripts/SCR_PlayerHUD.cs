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
   }

   private void SetTextHealth(float value)
   {
      float valuePercent = value * 100;
      healthText.SetText($"{(int)valuePercent}/100");
   }
   private void SetTextBlood(float value)
   {
      bloodText.SetText($"Blood: {(int)value}");
   }
   
   private void SetTextBaseHealth(float value)
   {
      float valuePercent = value * 100;
      baseHealthText.SetText($"{(int)valuePercent}/100");
   }
}
