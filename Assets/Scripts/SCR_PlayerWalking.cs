using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SCR_PlayerWalking : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject characterModel;
    private float currentHorizontalMovement;
    private float currentVerticalMovement;
    private float previousHorizontalMovement;
    private float previousVerticalMovement;
    

    private void Awake()
    {
        previousHorizontalMovement = characterModel.transform.localPosition.x;
        previousVerticalMovement = characterModel.transform.localPosition.y;
    }

    private void Update()
    {
        currentHorizontalMovement = gameObject.transform.position.x;
        currentVerticalMovement = gameObject.transform.position.y;
        

        if (previousHorizontalMovement < currentHorizontalMovement)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (previousHorizontalMovement > currentHorizontalMovement)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (previousVerticalMovement < currentVerticalMovement)
        {
            anim.SetBool("movedUp", true);
        }
        else if (previousVerticalMovement > currentVerticalMovement)
        {
            anim.SetBool("movedUp", false);
        }
        
        previousHorizontalMovement = currentHorizontalMovement;
        previousVerticalMovement = currentVerticalMovement;
    }
}
