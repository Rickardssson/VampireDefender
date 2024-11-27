using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SCREnemyMovement2 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float screenBorder;
    [SerializeField] private float obstacleCheckCircleRadius;
    [SerializeField] private float obstacleCheckDistance;
    [SerializeField] private LayerMask obstacleLayerMask;
        
    private Rigidbody2D rigidbody;
    private PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;
    private Vector2 obstacleAvoidanceTargetDirection;
    private Vector2? lastKnownPlayerPosition = null;
    private float obstacleAvoidanceCooldown;
    private float changeDirectionCooldown;
    private Camera camera;
    private RaycastHit2D[] obstacleCollisions;
    
    private Vector2 PlayerPosition => PlayerPosition;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<PlayerAwarenessController>();
        targetDirection = transform.up;
        camera = Camera.main;
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
        /*HandleRandomDirectionChange();*/
        HandlePlayerTargeting();
        HandleObstacles();
        HandleEnemyOffScreen();
    }
    
    private void HandleRandomDirectionChange()
    {
        changeDirectionCooldown -= Time.deltaTime;

        if (changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;
            
            changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }
    
    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < 0 && targetDirection.x < 0) || 
            (screenPosition.x > camera.pixelWidth && targetDirection.x > 0))
        {
            targetDirection = new Vector2(-targetDirection.x, targetDirection.y);
        }
        
        if ((screenPosition.y < 0 && targetDirection.y < 0) || 
            (screenPosition.y > camera.pixelHeight && targetDirection.y > 0))
        {
            targetDirection = new Vector2(targetDirection.x, -targetDirection.y);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (playerAwarenessController.AwareOfPlayer)
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
    
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
            rigidbody.velocity = transform.up * speed;
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
