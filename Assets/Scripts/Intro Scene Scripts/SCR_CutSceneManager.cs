/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CutSceneManager : MonoBehaviour
{
    private float guiAlpha;
    void Start()
    {
        PlayerPrefs.SetString("HasSeenIntro", "yes");
        GUIFade(0, 1, 1); // Start, end, length in seconds
    }
    

    

    void OnGUI () {
        GUI.color.a = guiAlpha;
        if (GUILayout.Button("Click me to fade out")) {
            GUIFade(1, 0, 2);
        }
    }

    void GUIFade (float start, float end, float length) { 
        for (i = 0.0; i <= 1.0; i += Time.deltaTime*(1/length)) { 
            guiAlpha = Mathf.Lerp(start, end, i); 
            return; 
        }
        guiAlpha = end; // Accounts for Time.deltaTime variance
    }
}
*/
