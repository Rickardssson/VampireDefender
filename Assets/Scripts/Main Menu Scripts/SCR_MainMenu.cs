using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_MainMenu : MonoBehaviour
{
    public void Play()
    {
        //Kollar om spelaren har sett intro cutscenen.
        /*
         if (PlayerPrefs.HasKey("HasSeenIntro"))
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            SceneManager.LoadScene("IntroCutScene");
        }
        */
        
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
