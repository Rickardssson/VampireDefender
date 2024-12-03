using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRBuildingManager : MonoBehaviour
{
    public static SCRBuildingManager main;

    [Header("References")]
    //[SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private Tower[] towers;
    
    private int selectedTower = 0;
    public int currency;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
    }
    
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            //buy item
            currency -= amount;
            return true;
        }
        else
        {   
            Debug.Log("Poor sod");
            return false;
        }
    }


    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
    
    

}
