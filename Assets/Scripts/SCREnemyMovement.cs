using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCREnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minDirectionChangeTime = 1f;
    [SerializeField] private float _maxDirectionChangeTime = 5f;
    [SerializeField] private float _waitTime = 2f;
    
    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    
    private Vector2 _randomDirection;
    private float _changeDirectionCooldown;
    private bool _isWaiting;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();

        if (_playerAwarenessController == null)
        {
            Debug.LogError("PlayerAwareness Controller not found");
        }

        SetRandomDirection();
    }
    
    private void FixedUpdate()
    {
        HandleRandomDirectionChange();
        if (_playerAwarenessController != null && _playerAwarenessController.AwareOfPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            HandleRandomDirectionChange();
        }
    }
    
    private void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = _playerAwarenessController.DirectionToPlayer;
        _rigidbody.velocity = directionToPlayer * _speed;
    }
    
    private void HandleRandomDirectionChange()
    {
        if (_isWaiting)
        {
            StopMovement();
        }
        else
        {
            _rigidbody.velocity = _randomDirection * _speed;
            _changeDirectionCooldown -= Time.deltaTime;
            
            if (_changeDirectionCooldown <= 0)
            {
                StartCoroutine(WaitBeforeMovement());
            }
        }
    }

    private IEnumerator WaitBeforeMovement()
    {
        // Enter waiting state
        _isWaiting = true;
        StopMovement();
        
        // Waiting State
        yield return new WaitForSeconds(_waitTime);
        
        // Exit waiting state
        SetRandomDirection();
        _isWaiting = false;
    }

    private void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        _randomDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            ).normalized;
        _changeDirectionCooldown = Random.Range(_minDirectionChangeTime, _maxDirectionChangeTime);
    }

    private void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
