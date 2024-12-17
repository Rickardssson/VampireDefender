using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_TutorialHandler : MonoBehaviour
{
    private bool isGamePaused;
    public GameObject Canvas;
    
    void Start()
    {
        PauseGame();
    }

    public void Tutorialbutton()
    {
        if (!isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Canvas.gameObject.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0;
        
    }

    private void ResumeGame()
    {
        isGamePaused = false;
        Canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
        
    }
}
