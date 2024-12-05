using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCR_PlayerHUD : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI healthText, bloodText;
   [SerializeField] private SCR_BuildingManager buildingManager;
   private GameObject player;
   private float playerRemHealth;
   private float playerRemBlood;
   
   private SCR_PlayerHealth _playerHealth;

   private void Start()
   {
      player = GameObject.FindGameObjectWithTag("Player");
      _playerHealth = player.GetComponent<SCR_PlayerHealth>();
      
   }

   private void Update()
   {
      playerRemHealth = _playerHealth.RemainingHealthPercentage; 
      playerRemBlood =  buildingManager.currency;
      SetTextHealth(playerRemHealth);
      SetTextBlood(playerRemBlood);
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
}
