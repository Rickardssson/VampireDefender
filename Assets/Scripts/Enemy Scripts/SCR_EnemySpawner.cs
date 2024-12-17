using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SCR_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject P_Enemy;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private float spawnRange = 5f;
    [SerializeField] private int spawnLimit = 4;
    [SerializeField] private SCR_DayNightCycle dayNightCycle;
    
    private float _timeToSpawn;
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
        setTimeUntilSpawn();
        isSpawning = true;
        StartCoroutine(EnemySpawn());
    }
    
    IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (dayNightCycle.Hours >= 5 && dayNightCycle.Hours < 17)
            {
                if (numberOfEnemies < spawnLimit)
                {
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-spawnRange, spawnRange), 
                        1, 
                        Random.Range(-spawnRange,spawnRange)
                    );
                    
                    Vector3 spawnPosition = transform.position + randomOffset;
                    spawnPosition.z = transform.position.z;
                
                    GameObject newEnemy = Instantiate(P_Enemy, spawnPosition, Quaternion.identity);
                    SCR_EnemyHealth enemyHealth = newEnemy.GetComponent<SCR_EnemyHealth>();
                
                    if (enemyHealth == null)
                    {
                        Debug.LogWarning("Enemy doesn't have SCR_EnemyHealth Component");
                    }
                    
                    numberOfEnemies++;
                }

                if (numberOfEnemies >= spawnLimit)
                {
                    isSpawning = false;
                    Destroy(gameObject);
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
    
    private void setTimeUntilSpawn()
    {
        _timeToSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
