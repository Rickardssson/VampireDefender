using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _timeToWaitBeforeExit;
    
    public void OnPlayerDeath()
    {
        Invoke(nameof(EndGame), _timeToWaitBeforeExit);
    }
    
    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
