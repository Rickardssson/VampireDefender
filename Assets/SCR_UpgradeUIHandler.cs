using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver = false;

    private void Update()
    {
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
