using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SCR_UIManager : MonoBehaviour
{
    public static SCR_UIManager main;
    

    private bool isHoveringUI;
    private void Awake()
    {
        main = this;
    }
    

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }
}
