using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Menu : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private Animator anim;
    

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("MenuOpen", true);
        }
        else
        {
            anim.SetBool("MenuOpen", false);
        }
    }
    

}
