using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SCR_EnemyMovement2 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float screenBorder;
    [SerializeField] private float obstacleCheckCircleRadius;
    [SerializeField] private float obstacleCheckDistance;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float minDirectionChangeTime = 1f;
    [SerializeField] private float maxDirectionChangeTime = 5f;
    [SerializeField] private bool enableRandomDirections = true;
        
    private Rigidbody2D _rigidbody;
    private SCR_PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;
    private Vector2 obstacleAvoidanceTargetDirection;
    private Vector2? lastKnownPlayerPosition = null;
    private float obstacleAvoidanceCooldown;
    private float changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] obstacleCollisions;
    private bool _isWaiting;
    
    private Vector2 PlayerPosition => playerAwarenessController.PlayerPosition;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<SCR_PlayerAwarenessController>();
        targetDirection = transform.up;
        _camera = Camera.main;
        obstacleCollisions = new RaycastHit2D[10];
    }
    
    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }
    
    private void HandleRandomDirectionChange()
    {
        if (!enableRandomDirections) return;
        
        if (_isWaiting)
        {
            StopMovement();
        }
        else
        {
            _rigidbody.velocity = targetDirection * speed;
            changeDirectionCooldown -= Time.deltaTime;
            
            if (changeDirectionCooldown <= 0)
            {
                StartCoroutine(WaitBeforeMovement());
            }
        }
    }
    
    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < 0 && targetDirection.x < 0) || 
            (screenPosition.x > _camera.pixelWidth && targetDirection.x > 0))
        {
            targetDirection = new Vector2(-targetDirection.x, targetDirection.y);
        }
        
        if ((screenPosition.y < 0 && targetDirection.y < 0) || 
            (screenPosition.y > _camera.pixelHeight && targetDirection.y > 0))
        {
            targetDirection = new Vector2(targetDirection.x, -targetDirection.y);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (playerAwarenessController.AwareOfPlayer && playerAwarenessController.HasLineOfSight)
        {
            targetDirection = playerAwarenessController.DirectionToPlayer;
            lastKnownPlayerPosition = playerAwarenessController.PlayerPosition;
        }
        else if (lastKnownPlayerPosition.HasValue)
        {
            Vector2 direction = (lastKnownPlayerPosition.Value - (Vector2)transform.position).normalized;
            targetDirection = direction;

            if (Vector2.Distance(transform.position, lastKnownPlayerPosition.Value) < 0.5f)
            {
                lastKnownPlayerPosition = null;
            }
        }
        else
        {
            HandleRandomDirectionChange();
        }
    }

    private void HandleObstacles()
    {
        
        obstacleAvoidanceCooldown -= Time.deltaTime;
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(obstacleLayerMask);
        
        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            obstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            obstacleCollisions,
            obstacleCheckDistance
            );

        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollide = this.obstacleCollisions[index];

            if (obstacleCollide.collider.gameObject == gameObject)
            {
                continue;
            }

            if (obstacleAvoidanceCooldown <= 0)
            {
                obstacleAvoidanceTargetDirection = obstacleCollide.normal;
                obstacleAvoidanceCooldown = 0.5f;
            }
            
            var targetRotation = Quaternion.LookRotation(
                transform.forward, 
                obstacleAvoidanceTargetDirection
                );
            var rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, 
                rotationSpeed * Time.deltaTime
                );
            
            targetDirection = rotation * Vector2.up;
            break;
        }
    }
    
    private IEnumerator WaitBeforeMovement()
    {
        if (!enableRandomDirections) yield break;
        
        // Enter waiting state
        _isWaiting = true;
        StopMovement();
        
        // Waiting State
        yield return new WaitForSeconds(waitTime);
        
        // Exit waiting state
        SetRandomDirection();
        _isWaiting = false;
    } 
    
    private void SetRandomDirection()
    {
        if (!enableRandomDirections) return;
        
        float randomAngle = Random.Range(0f, 360f);
        targetDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        ).normalized;
        changeDirectionCooldown = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
    }
    
    private void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
    }
    
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
            _rigidbody.velocity = transform.up * speed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, obstacleCheckCircleRadius);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, obstacleCheckDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)targetDirection * 2f);
        
        if (lastKnownPlayerPosition.HasValue)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(lastKnownPlayerPosition.Value, 0.3f);
        }
    }
}
