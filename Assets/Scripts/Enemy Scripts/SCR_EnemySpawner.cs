using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SCR_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject P_Enemy;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private float spawnRange = 5f;
    [SerializeField] private int spawnLimit = 4;
    [SerializeField] private int maxEnemiesPerDay = 10;
    [SerializeField] private SCR_DayNightCycle dayNightCycle;
    [SerializeField] private GameManager gameManager;
    
    private float _timeToSpawn;
    private int numberOfEnemiesToday;
    private int originalSpawnLimit;
    public bool isSpawning { get; private set; }
    public int numberOfEnemies { get; private set; }
    
    void Awake()
    {
        if (dayNightCycle == null)
        {
            dayNightCycle = FindObjectOfType<SCR_DayNightCycle>();

            if (dayNightCycle == null)
            {
                Debug.LogError("SCR_DayNightCycle not found in the scene!");
            }
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            
            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in the scene!");
            }
        }
        
        originalSpawnLimit = spawnLimit;
        setTimeUntilSpawn();
        isSpawning = true;
        StartCoroutine(EnemySpawn());
        StartCoroutine(DestroySpawner());
    }
    
    IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (dayNightCycle.Hours >= 5 && dayNightCycle.Hours < 17)
            {
                if (numberOfEnemiesToday < maxEnemiesPerDay && spawnLimit > 0)
                {
                    SpawnEnemy();
                }
            }
            else
            {
                isSpawning = false;
            }
            
            yield return new WaitForSeconds(_timeToSpawn);
            setTimeUntilSpawn();
        }
    }

    private void SpawnEnemy()
    {
        if (spawnLimit <= 0) return;
        
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRange, spawnRange), 
            1, 
            Random.Range(-spawnRange,spawnRange)
        );
                    
        Vector3 spawnPosition = transform.position + randomOffset;
        spawnPosition.z = transform.position.z;
            
        GameObject newEnemy = Instantiate(P_Enemy, spawnPosition, Quaternion.identity);
        spawnLimit--;
        numberOfEnemiesToday++;
    }
    
    private void setTimeUntilSpawn()
    {
        _timeToSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
    
    IEnumerator DestroySpawner()
    {
        yield return new WaitUntil(() => dayNightCycle.Days >= gameManager.maxDays);
        Destroy(gameObject);
    }

    public void ResetDailySpawnCount()
    {
        numberOfEnemiesToday = 0;
        isSpawning = true;
        maxEnemiesPerDay = Mathf.CeilToInt((float)maxEnemiesPerDay * 1.3f);
        minSpawnTime -= 0.9f;
        maxSpawnTime -= 0.9f;
        if (maxSpawnTime <= 2) maxSpawnTime = 1.8f;
        if (minSpawnTime <= 0.9f) minSpawnTime = 0.9f;
        spawnLimit = maxEnemiesPerDay;
        
        
        StopCoroutine(EnemySpawn());
        StartCoroutine(EnemySpawn());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
