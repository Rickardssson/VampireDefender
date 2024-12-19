using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SCR_Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color spaceOccupiedColor;
    [SerializeField] private Color showColor;
    
    [Header("Audio Event")]
    public UnityEvent onTowerBuilt;
    
    private GameObject towerObj;
    public SCR_Turret turret;
    private Color startColor;
    private bool pressedKey;

    private void Start()
    {
        startColor = sr.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && pressedKey == false)
        {
            sr.color = showColor;
            pressedKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && pressedKey == true)
        {
            sr.color = startColor;
            pressedKey = false;
        }
    }


    private void OnMouseEnter()
    {
        if (towerObj)
        {
            OccupiedSpace();
        }
        else
        {
            ShowMenu();
        }
        
    }

    private void OnMouseExit()
    {
        if (pressedKey == true)
        {
            sr.color = showColor;
        }

        return;
    }

    private void OnMouseDown()
    {
        if(SCR_UIManager.main.IsHoveringUI()) return;
        BuildTower();
    }

    private void ShowMenu()
    {
        if (pressedKey == true)
        {
            sr.color = hoverColor;
        }
        else if (pressedKey == false)
        {
            sr.color = startColor;
        }
        
    }

    private void OccupiedSpace()
    {
        if (pressedKey == true)
        {
            sr.color = spaceOccupiedColor;
        }
    }

    private void BuildTower()
    {
        if (pressedKey == true)
        {
            sr.color = hoverColor;
            if (towerObj != null)
            {
                turret.OpenUpgradeUI();
                return;
            }
        
            SCR_Tower towerToBuild = SCR_BuildingManager.main.GetSelectedTower();

            if (towerToBuild.cost > SCR_BuildingManager.main.currency)
            {
                Debug.Log("Giga Poor");
                return;
            }
            
            SCR_BuildingManager.main.SpendCurrency(towerToBuild.cost);
            sr.color = spaceOccupiedColor;
            towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            turret = towerObj.GetComponent<SCR_Turret>();
            
            onTowerBuilt?.Invoke();
        }
    }
}
