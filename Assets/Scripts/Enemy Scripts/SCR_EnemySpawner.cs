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
    
    private float _timeToSpawn;
    public bool isSpawning { get; private set; }
    public int numberOfEnemies { get; private set; }
    
    void Awake()
    {
        setTimeUntilSpawn();
        isSpawning = true;
        StartCoroutine(EnemySpawn());
    }
    
    IEnumerator EnemySpawn()
    {
        while (isSpawning)
        {
            if (numberOfEnemies < spawnLimit)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnRange, spawnRange), 
                    1, 
                    Random.Range(-spawnRange,spawnRange)
                );
                Vector3 spawnPosition = transform.position + randomOffset;
                
                Instantiate(P_Enemy, spawnPosition, Quaternion.identity);
                numberOfEnemies++;

                if (numberOfEnemies >= spawnLimit)
                {
                    isSpawning = false;
                    Destroy(gameObject);
                }
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
