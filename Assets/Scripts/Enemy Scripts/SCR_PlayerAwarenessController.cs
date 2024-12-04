using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class SCR_PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public bool HasLineOfSight => hasLineOfSight;
    public Vector2 DirectionToPlayer { get; private set; }
    public Vector2 PlayerPosition => _player.position;

    [SerializeField] private float _playerAwarenessDistance;

    private float losCooldownTimer;
    private Transform _player;
    private bool hasLineOfSight;
    private bool previousOnScreenState;

    private void Awake()
    {
        _player = FindObjectOfType<TopDownMovement>().transform;
        previousOnScreenState = false;
    }

    private void FixedUpdate()
    {
        int layerMask = LayerMask.GetMask("Player", "Object");
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        
        RaycastHit2D ray = Physics2D.Raycast(
            transform.position, 
            direction, 
            _playerAwarenessDistance, 
            layerMask
            );
        
        if (ray.collider != null && ray.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hasLineOfSight = true;
            losCooldownTimer = 1f;
            
            /*hasLineOfSight = ray.collider.gameObject.layer == LayerMask.NameToLayer("Player");
            if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position,
                    direction * (_playerAwarenessDistance),
                    hasLineOfSight ? Color.green: Color.red
                    );
            }
            else
            {
                Debug.DrawRay(transform.position, 
                    direction * (_playerAwarenessDistance), 
                    Color.red
                    );
            }*/
        }
        else if (losCooldownTimer > 0f)
        {
            losCooldownTimer -= Time.fixedDeltaTime;
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
        }
        
        Debug.DrawRay(
            transform.position, 
            direction * _playerAwarenessDistance, 
            hasLineOfSight ? Color.green : Color.red
            );
    }

    private void Update()
    {
        Vector2 enemytoPlayerVector = _player.position - transform.position;
        DirectionToPlayer = enemytoPlayerVector.normalized;

        if (!hasLineOfSight) return;

        AwareOfPlayer = enemytoPlayerVector.magnitude <= _playerAwarenessDistance && 
                        HasLineOfSight && 
                        IsEnemyOnScreen();

        bool isOnScreen = IsEnemyOnScreen();

        if (isOnScreen != previousOnScreenState)
        {
            Debug.Log(isOnScreen ? "Enemy is on-screen" : "Enemy is off-screen");
            previousOnScreenState = isOnScreen;
        }
    }

    private bool IsEnemyOnScreen()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
               viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
               viewportPosition.z >= 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerAwarenessDistance);

        if (Application.isPlaying)
        {
            Gizmos.color = IsEnemyOnScreen() ? Color.green : Color.gray;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
