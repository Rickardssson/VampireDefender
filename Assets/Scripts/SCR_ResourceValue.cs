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
            if (pickup == true && !isDestroyed)
            {
                other.gameObject.GetComponent<SCR_BuildingManager>().IncreaseCurrency(currencyWorth);
                isDestroyed = true;
                Destroy(gameObject);
            }
            else
            {
                return;
            } 
        }
    }
}
