using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_BuildingManager : MonoBehaviour
{
    public static SCR_BuildingManager main;

    [Header("References")]
    //[SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] public SCR_Tower[] towers;

    [SerializeField] public GameObject PickupPrefab;
    
    
    private int selectedTower = 0;
    public int currency;
    private bool pressedKey;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 12;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            pressedKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && pressedKey == true)
        {
            pressedKey = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && pressedKey == true)
        {
            selectedTower = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && pressedKey == true)
        {
            selectedTower = 1;
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        this.GetComponent<AudioSource>().Play();
        Instantiate(PickupPrefab, transform.position, Quaternion.identity);
        
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


    public SCR_Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
    
    

}
