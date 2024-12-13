using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SCR_Turret : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    
    
    [Header("Attribute")] 
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationspeed = 100f;
    [SerializeField] public float fireRate = 1f; // times it fires per second
    [SerializeField] private float baseUpgradeCost = 20; // base upgrade cost
    [SerializeField] private float fireRateUpgradeMultiplier = 2f;
    [SerializeField] private float targetingRangeUpgradeMultiplier = 2f;
    


    private Transform target;
    private float timeUntilFire;
    private int level = 1;


    private void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
    }
    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / fireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }

        RotateTowardsTarget();
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        SCR_Bullet bulletScript = bulletObj.GetComponent<SCR_Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)
            transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;
        
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        SCR_UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalulateUpgradeCost() > SCR_BuildingManager.main.currency) return;
        
        SCR_BuildingManager.main.SpendCurrency(CalulateUpgradeCost());

        level++;

        fireRate = CalculateFireRate();
        targetingRange = CalculateTargetingRange();
        CloseUpgradeUI();
        Debug.Log("upgraded turret!");
        Debug.Log("new firerate:" + fireRate);
        Debug.Log("new targeting range" + targetingRange);
        Debug.Log("new cost!" + CalulateUpgradeCost());
    }

    private float CalculateFireRate()
    {
        return fireRate * fireRateUpgradeMultiplier;
    }
    
    private float CalculateTargetingRange()
    {
        return targetingRange * targetingRangeUpgradeMultiplier;
    }

    private int CalulateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    public float ShowFireRateUI()
    {
            return fireRate;
    }

    public float ShowTargetRangeUI()
    {
        return targetingRange;
    }
    
}
