using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color spaceOccupiedColor;
    
    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if (tower)
        {
            sr.color = spaceOccupiedColor;
        }
        else
        {
            sr.color = hoverColor;
        }
        
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;

        GameObject towerToBuild = SCRBuildingManager.main.GetSelectedTower();
        tower = Instantiate(towerToBuild, transform.position, Quaternion.identity);
    }
}
