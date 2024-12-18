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
    [SerializeField] private int numberOfWinsToWin = 3;
    [SerializeField] public int maxDays = 3;
    
    private List<SCR_EnemySpawner> allSpawners = new List<SCR_EnemySpawner>();

    private int winCount = 0;
    
    private void Start()
    {
        SCR_DayNightCycle dayNightCycle = FindObjectOfType<SCR_DayNightCycle>();
        if (dayNightCycle != null)
        {
            dayNightCycle.OnDayEnd += HandleDayEnd;
        }
        
        allSpawners.AddRange(FindObjectsOfType<SCR_EnemySpawner>());
        
        if (WinScreen != null) WinScreen.SetActive(false);
        if (LoseScreen != null) LoseScreen.SetActive(false);
        
        InvokeRepeating(nameof(CheckWinCondition), 1f, 1f);
        InvokeRepeating(nameof(CheckLoseCondition), 1f, 1f);
    }

    private void HandleDayEnd(int curentDay)
    {
        if (curentDay < maxDays)
        {
            OnNewDay();
        }
        else
        {
            EndGameWithDayLimit();
        }
    }

    private void EndGameWithDayLimit()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnNewDay()
    {
        foreach (var spawner in allSpawners)
        {
            if (spawner != null)
            {
                spawner.ResetDailySpawnCount();
            }
        }
    }
    
    public void OnPlayerDeath()
    {
        if (LoseScreen != null)
        {
            LoseScreen.SetActive(true);
        }
        
        Invoke(nameof(EndGame), _timeToWaitBeforeExit);
    }

    public void WinRound()
    {
        winCount++;
        if (winCount >= numberOfWinsToWin)
        {
            EndGameWithVictory();
        }
    }

    private void EndGameWithVictory()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
        }
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
            OnPlayerDeath();
        }
    }

    private void CheckWinCondition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool allSpawnersDisabled = true;
        
        foreach (var spawner in allSpawners)
        {
            if (spawner != null)
            {
                allSpawnersDisabled = false;
                break;
            }
        }
        
        if (allSpawnersDisabled && enemies.Length <= 0)
        {
            WinRound();
        }
    }
}
