using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private GameObject turret;
    
    private float _upgradeAmount;
    
    public bool mouseOver = false;
    
    private SCR_Turret _turret;

    private void Update()
    {    
        _turret = turret.GetComponent<SCR_Turret>();
        _upgradeAmount = _turret.CalulateUpgradeCost();
        upgradeText.SetText("Upgrade: " + _upgradeAmount);
        
        if (mouseOver) Debug.Log("Mouse over" + name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        SCR_UIManager.main.SetHoveringState(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        SCR_UIManager.main.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
