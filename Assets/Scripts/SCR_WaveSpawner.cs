using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_WaveSpawner : MonoBehaviour
{
    public Transform P_Enemy;

    public Transform SpawnPoint;
    
    [SerializeField] public float TimeBetweenWaves = 5f;
    [SerializeField] public float SpawnRate = 0.4f;

    public Text WaveCountdownText;
    
    private float countDown = 2f;
    private int waveCount = 1;

    void Update()
    {
        if (countDown <= 0)
        {
            StartCoroutine(SpawnWave());
            countDown = TimeBetweenWaves;
        }
        
        countDown -= Time.deltaTime;
        
        WaveCountdownText.text = Mathf.Round(countDown).ToString();
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(SpawnRate);
        }
        
        waveCount++;
    }

    void SpawnEnemy()
    {
        Instantiate(P_Enemy, SpawnPoint.position, SpawnPoint.rotation);
    }
}
