using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SCR_EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reactionSpeed = 10f;
    [SerializeField] private bool _enableRandomDirections = true;
    [SerializeField] private float _minDirectionChangeTime = 1f;
    [SerializeField] private float _maxDirectionChangeTime = 5f;
    [SerializeField] private float _waitTime = 2f;
    [SerializeField] private float _screenBorder;
    [SerializeField] private float _startDirection;
    [SerializeField] private float _obstacleCheckCircleRadius;
    [SerializeField] private float _obstacleCheckDistance;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Rigidbody2D _rigidbody;
    private SCR_PlayerAwarenessController _playerAwarenessController;
    
    /*private Vector2 _randomDirection;*/
    private Vector2 _currentDirection;
    private Vector2 _lastValidDirection;
    private float _changeDirectionCooldown;
    private bool _isWaiting;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollision;
    private float _obstacleAvoidanceCooldownTime;
    private Vector2 _obstacleAvoidanceTargetDirection;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<SCR_PlayerAwarenessController>();
        _camera = Camera.main;
        _obstacleCollision = new RaycastHit2D[10];

        if (_playerAwarenessController == null)
        {
            Debug.LogError("PlayerAwareness Controller not found");
        }

        SetStartDirection();
        /*SetRandomDirection();*/
    }
    
    private void FixedUpdate()
    {
        HandleRandomDirectionChange();
        HandleObstacles();
        
        if (_playerAwarenessController != null && _playerAwarenessController.AwareOfPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            HandleRandomDirectionChange();
            HandleEnemyOffScreen();
        }
    }
    
    private void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = _playerAwarenessController.DirectionToPlayer;
        _rigidbody.velocity = directionToPlayer * _speed;
    }
    
    private void HandleRandomDirectionChange()
    {
        if (!_enableRandomDirections) return;
        
        if (_isWaiting)
        {
            StopMovement();
        }
        else
        {
            _rigidbody.velocity = _currentDirection * _speed;
            _changeDirectionCooldown -= Time.deltaTime;
            
            if (_changeDirectionCooldown <= 0)
            {
                StartCoroutine(WaitBeforeMovement());
            }
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < 0 && _rigidbody.velocity.x < 0) || 
            (screenPosition.x > _camera.pixelWidth && _rigidbody.velocity.x > 0))
        {
            _rigidbody.velocity = new Vector2(-_rigidbody.velocity.x, _rigidbody.velocity.y);
        }
        
        if ((screenPosition.y < 0 && _rigidbody.velocity.y < 0) || 
            (screenPosition.y > _camera.pixelHeight && _rigidbody.velocity.y > 0))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_rigidbody.velocity.y);
        }
    }

    private void HandleObstacles()
    {
        _obstacleAvoidanceCooldownTime = Time.deltaTime;
        
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_obstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _obstacleCheckCircleRadius,
            Vector2.zero,
            contactFilter,
            _obstacleCollision,
            _obstacleCheckDistance);

        if (numberOfCollisions > 0)
        {
            for (int index = 0; index < numberOfCollisions; index++)
            {
                var obstacleCollision = _obstacleCollision[index];

                if (obstacleCollision.collider.gameObject == gameObject)
                {
                    continue;
                }

                if (_obstacleAvoidanceCooldownTime <= 0)
                {
                    _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                    _obstacleAvoidanceCooldownTime = 0.5f;
                }
                
                // Calculate "avoidance direction" (the objects new forward direction)
                /*Vector2 avoidanceDirection = obstacleCollision.normal; */
            
                // Rotate current direction to avoidance direction
                _currentDirection = Vector2.Lerp(
                    _currentDirection, _obstacleAvoidanceTargetDirection, 
                    Time.fixedDeltaTime * _reactionSpeed
                    ).normalized;
                return;
            }   
        }
        else
        {
            _currentDirection = _lastValidDirection;
        }
        
        _lastValidDirection = _currentDirection;
    }

    private IEnumerator WaitBeforeMovement()
    {
        if (!_enableRandomDirections) yield break;
        
        // Enter waiting state
        _isWaiting = true;
        StopMovement();
        
        // Waiting State
        yield return new WaitForSeconds(_waitTime);
        
        // Exit waiting state
        SetRandomDirection();
        _isWaiting = false;
    }

    private void SetStartDirection()
    {
        _currentDirection = new Vector2(
            Mathf.Cos(_startDirection * Mathf.Deg2Rad), 
            Mathf.Sin(_startDirection * Mathf.Deg2Rad)
            ).normalized;

        _lastValidDirection = _currentDirection;
        _changeDirectionCooldown = Random.Range(_minDirectionChangeTime, _maxDirectionChangeTime);
    }
    
    private void SetRandomDirection()
    {
        if (!_enableRandomDirections) return;
        
        float randomAngle = Random.Range(0f, 360f);
        _currentDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
            ).normalized;
        _changeDirectionCooldown = Random.Range(_minDirectionChangeTime, _maxDirectionChangeTime);
    }

    private void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _obstacleCheckCircleRadius);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _obstacleCheckDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_currentDirection * 2f);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_lastValidDirection * 2f);
    }
}
