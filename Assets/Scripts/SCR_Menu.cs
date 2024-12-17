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
    [SerializeField] private GameObject buildingManager;
    [SerializeField] private CanvasRenderer basicTurretImage;
    [SerializeField] private CanvasRenderer testTurretImage;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color regularColor;
    
    private float basicTurretFireRate;
    private float basicTurretRange;
    private float basicTurretCost;
    private float testTurretFireRate;
    private float testTurretRange;
    private float testTurretCost;
    private SCR_Turret _basicTurret;
    private SCR_Turret _testTurret;
    private SCR_BuildingManager _buildingManager;
    private bool pressedKey;

    

    private void Start()
    {
        GetBuildingManagerCostInfo();
        GetBasicTurretStats();
        GetTestTurretStats();
        ShowTurretStats();

    }
    public void Update()
    {
        HiglightTurret();
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
        
    }

    private void GetBasicTurretStats()
    {
        basicTurretFireRate = _basicTurret.ShowFireRateUI();
        basicTurretRange = _basicTurret.ShowTargetRangeUI();
    }
    private void GetTestTurretStats()
    {
        testTurretFireRate = _testTurret.ShowFireRateUI();
        testTurretRange = _testTurret.ShowTargetRangeUI();
    }

    public void GetBuildingManagerCostInfo()
    {
        _basicTurret = basicTurret.GetComponent<SCR_Turret>();
        _testTurret = testTurret.GetComponent<SCR_Turret>();
        _buildingManager = buildingManager.GetComponent<SCR_BuildingManager>();
        basicTurretCost = _buildingManager.towers[0].cost;
        testTurretCost = _buildingManager.towers[1].cost;
    }

    private void ShowTurretStats()
    {
        firstTurretInfo.SetText("Basic Turret Firerate:" + basicTurretFireRate + " Range:" + basicTurretRange + " Cost:" + basicTurretCost);
        secondTurretInfo.SetText("Test Turret Firerate:" + testTurretFireRate + " Range:" + testTurretRange + " Cost:" + testTurretCost);
    }
    

    private void HiglightTurret()
    {
        if (pressedKey == true && Input.GetKeyDown(KeyCode.Alpha1))
        {
            basicTurretImage.SetColor(highlightColor);
            testTurretImage.SetColor(regularColor);
        }
        else if (pressedKey == true && Input.GetKeyDown(KeyCode.Alpha2))
        {
            testTurretImage.SetColor(highlightColor);
            basicTurretImage.SetColor(regularColor);
        }
    }

}
