using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _timeToWaitBeforeExit;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    
    private void Start()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("WinScreen GameObject not assigned in GameManager");
        }
        
        if (LoseScreen != null)
        {
            LoseScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("WinScreen GameObject not assigned in GameManager");
        }
        
        InvokeRepeating(nameof(CheckWinCondition), 1f, 1f);
        InvokeRepeating(nameof(CheckLoseCondition), 1f, 1f);
    }

    
    public void OnPlayerDeath()
    {
        if (LoseScreen != null)
        {
            LoseScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("LoseScreen GameObject not assigned in GameManager");
        }
        
        Invoke(nameof(EndGame), _timeToWaitBeforeExit);
    }
    
    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void CheckLoseCondition()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        if (player.Length <= 0)
        {
            if (LoseScreen != null)
            {
                LoseScreen.SetActive(true);
            }
        }
    }

    private void CheckWinCondition()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (spawners.Length <= 0 && enemies.Length <= 0)
        {
            if (WinScreen != null)
            {
                WinScreen.SetActive(true);
            }
        }
    }
}
