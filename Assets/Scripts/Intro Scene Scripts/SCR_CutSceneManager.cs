
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
            child.gameObject.GetComponent<Graphic>().FadeIn(FadeTime);
        }
        fadeObject.GetComponent<Graphic>().FadeIn(FadeTime);
        PlayerPrefs.SetString("HasSeenIntro", "yes");
    }
}

