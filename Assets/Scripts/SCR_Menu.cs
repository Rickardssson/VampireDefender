using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SCR_Menu : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI firstTurretInfo, secondTurretInfo;
    [SerializeField] private GameObject basicTurret;
    [SerializeField] private GameObject testTurret;

    private float turretFireRate;
    private float turretRange;
    private float towerCost;
    private SCR_Turret _basicTurret;
    private SCR_Turret _testTurret;
    private bool pressedKey;

    

    private void Start()
    {
        _basicTurret = basicTurret.GetComponent<SCR_Turret>();
        _testTurret = testTurret.GetComponent<SCR_Turret>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && pressedKey == false)
        {
            anim.SetBool("MenuOpen", true);
            pressedKey = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && pressedKey == true)
        {
            anim.SetBool("MenuOpen", false);
            pressedKey = false;
        }
        
        GetTurretStats();
    }

    public void OpenMenu()
    {
        
    }

    private void GetTurretStats()
    {
        
    }
    

}
