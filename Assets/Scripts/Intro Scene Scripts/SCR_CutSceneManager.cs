
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SCR_CutSceneManager : MonoBehaviour
{
    [SerializeField] float FadeTime = 1;
    [SerializeField] GameObject fadeObject;
    void Start()
    {
        foreach (Transform child in fadeObject.transform)
        {
            SCR_UI_Fade.FadeIn(child.gameObject.GetComponent<Graphic>(), FadeTime);
        }
        SCR_UI_Fade.FadeIn(fadeObject.GetComponent<Graphic>(), FadeTime);
        SCR_MainMenu.hasSeenIntro = true;
    }
}

