using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public bool HasLineOfSight => hasLineOfSight;
    public Vector2 DirectionToPlayer { get; private set; }
    public Vector2 PlayerPosition => _player.position;

    [SerializeField] private float _playerAwarenessDistance;

    private float losCooldownTimer;
    private Transform _player;
    private bool hasLineOfSight;

    private void Awake()
    {
        _player = FindObjectOfType<TopDownMovement>().transform;
    }

    private void FixedUpdate()
    {
        int layerMask = LayerMask.GetMask("Player", "Default", "Object");
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
        
        AwareOfPlayer = enemytoPlayerVector.magnitude <= _playerAwarenessDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _playerAwarenessDistance);
    }
}
