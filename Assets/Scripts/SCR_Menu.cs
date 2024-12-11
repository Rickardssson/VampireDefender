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

    

    private void Start()
    {
        _basicTurret = basicTurret.GetComponent<SCR_Turret>();
        _testTurret = testTurret.GetComponent<SCR_Turret>();
    }
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
        
        GetTurretStats();
    }

    private void GetTurretStats()
    {
        
    }
    

}
