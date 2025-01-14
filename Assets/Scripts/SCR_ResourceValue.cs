using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ResourceValue : MonoBehaviour
{
    [Header("Attributes")] 
    [SerializeField] private int currencyWorth = 50;

    private bool pickup = false;
    private bool isDestroyed = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ResourcePickUp"))
        {
            Debug.Log("we are touching!");
            pickup = true;
            if (pickup && !isDestroyed)
            {
                SCR_BuildingManager buildingManager = other.gameObject.GetComponent<SCR_BuildingManager>();
                if (buildingManager != null)
                {
                    buildingManager.IncreaseCurrency(currencyWorth);
                    isDestroyed = true;
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("No SCR_BuildingManager component found on the object!");
                }
            }
        }
    }
}
