using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SCR_PlayerWalking : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private float currentHorizontalMovement;
    private float currentVerticalMovement;
    private float previousHorizontalMovement;
    private float previousVerticalMovement;

    private void Update()
    {
        previousHorizontalMovement = gameObject.transform.position.x;
        previousVerticalMovement = gameObject.transform.position.y;
        currentHorizontalMovement = gameObject.transform.position.x;
        currentVerticalMovement = gameObject.transform.position.y;

        if (previousHorizontalMovement < currentHorizontalMovement)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (previousHorizontalMovement > currentHorizontalMovement)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (previousVerticalMovement < currentVerticalMovement)
        {
            anim.SetBool("movedUp", true);
        }
        else if (previousVerticalMovement > currentVerticalMovement)
        {
            anim.SetBool("movedUp", false);
        }
        else return;
    }
}
