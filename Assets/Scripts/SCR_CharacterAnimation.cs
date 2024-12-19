using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animatorBack;
    [SerializeField] private Animator animatorFront;
    [SerializeField] private GameObject characterModel;
    [SerializeField] private GameObject vasiliaBackPrefab;
    [SerializeField] private GameObject vasiliaFrontPrefab;
    private float currentHorizontalMovement;
    private float currentVerticalMovement;
    private float previousHorizontalMovement;
    private float previousVerticalMovement;
    private bool enemyAttack;
    

    private void Awake()
    {
        enemyAttack = false;
        
        if (characterModel == null)
        {
            Transform childTransform = GetComponentInChildren<Transform>();
            if (childTransform != null)
            {
                characterModel = childTransform.gameObject;
            }
            else
            {
                Debug.LogError("Character model not assigned and no child GameObject found!");
                return;
            }
        }
        
        previousHorizontalMovement = characterModel.transform.localPosition.x;
        previousVerticalMovement = characterModel.transform.localPosition.y;
    }

    private void Update()
    {
        currentHorizontalMovement = gameObject.transform.position.x;
        currentVerticalMovement = gameObject.transform.position.y;
        
        if (previousHorizontalMovement < currentHorizontalMovement)
        {
            vasiliaBackPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
            vasiliaFrontPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (previousHorizontalMovement > currentHorizontalMovement)
        {
            vasiliaBackPrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
            vasiliaFrontPrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animatorFront.SetBool("Attacking", true);
            animatorBack.SetBool("Attacking", true);
            enemyAttack = false;
        }
        else
        {
            animatorFront.SetBool("Attacking", false);
            animatorBack.SetBool("Attacking", false);
        }
        if (enemyAttack)
        {
            animatorFront.SetBool("AttackingPlayer", true);
            animatorBack.SetBool("AttackingPlayer", true);
            enemyAttack = false;
        }
        else
        {
            animatorFront.SetBool("AttackingPlayer", false);
            animatorBack.SetBool("AttackingPlayer", false);
        }

        if (previousVerticalMovement < currentVerticalMovement)
        {
            vasiliaBackPrefab.SetActive(true);
            vasiliaFrontPrefab.SetActive(false);
        }
        else if (previousVerticalMovement > currentVerticalMovement)
        {
            vasiliaBackPrefab.SetActive(false);
            vasiliaFrontPrefab.SetActive(true);
        }
        else return;
        
        previousHorizontalMovement = currentHorizontalMovement;
        previousVerticalMovement = currentVerticalMovement;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Base" || other.gameObject.tag == "Player")
        {
            enemyAttack = true;
            Debug.Log("I played my animation!");
        }
        else return;
    }
}
