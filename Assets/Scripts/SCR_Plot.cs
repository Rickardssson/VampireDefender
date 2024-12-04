using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCR_Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color spaceOccupiedColor;
    [SerializeField] private Color showColor;
    
    
    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sr.color = showColor;
        }
        else if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            sr.color = startColor;
        }
    }


    private void OnMouseEnter()
    {
        if (tower)
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
        sr.color = showColor;
    }

    private void OnMouseDown()
    {
        BuildTower();
    }

    private void ShowMenu()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sr.color = hoverColor;
        }
        
    }

    private void OccupiedSpace()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sr.color = spaceOccupiedColor;
        }
    }

    private void BuildTower()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sr.color = hoverColor;
            if (tower != null)
            {
                sr.color = spaceOccupiedColor;
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
            tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        }
    }
}
