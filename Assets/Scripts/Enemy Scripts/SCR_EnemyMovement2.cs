using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SCR_EnemyMovement2 : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float obstacleCheckCircleRadius;
    [SerializeField] private float obstacleCheckDistance;
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float maxTimeWithoutLOS = 5f;
    [SerializeField] private float randomDirectionChangeTime = 1f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private bool enableRandomDirections = true;
    [SerializeField] private SCR_DayNightCycle dayNightCycle;

    public GameObject P_Enemy;
    
    private Rigidbody2D _rigidbody;
    private SCR_PlayerAwarenessController playerAwarenessController;
    private Vector2 targetDirection;
    private Vector2 obstacleAvoidanceTargetDirection;
    private Vector2? lastKnownPlayerPosition;
    private float timeSinceLOS;
    private float obstacleAvoidanceCooldown;
    private float changeDirectionCooldown;
    private bool isWaiting;
    private RaycastHit2D[] obstacleCollisions;
    private Vector2 smoothedVelocity;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAwarenessController = GetComponent<SCR_PlayerAwarenessController>();
        obstacleCollisions = new RaycastHit2D[10];
        SetRandomDirection();
    }
    
    void FixedUpdate()
    {
        if (!isWaiting)
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
            SetVelocity();
        }
    }

    private void UpdateTargetDirection()
    {
        HandleObstacles();
        HandlePlayerTargeting();

        if (lastKnownPlayerPosition.HasValue)
        {
            MoveToLastKnownPosition();
        }
        else
        {
            HandleRandomDirectionChange();
        }
       
    }
    
    private void HandlePlayerTargeting()
    {
        if (playerAwarenessController.AwareOfPlayer && playerAwarenessController.HasLineOfSight)
        {
            targetDirection = playerAwarenessController.DirectionToPlayer;
            lastKnownPlayerPosition = playerAwarenessController.PlayerPosition;
            timeSinceLOS = 0;
        }
        else
        {
            timeSinceLOS += Time.fixedDeltaTime;

            if (timeSinceLOS > maxTimeWithoutLOS)
            {
                targetDirection = (Vector2.zero - (Vector2)transform.position).normalized;
                lastKnownPlayerPosition = null;
            }
        }
    }

    private void MoveToLastKnownPosition()
    {
        Vector2 directionToLastPostion = (lastKnownPlayerPosition.Value - (Vector2)transform.position).normalized;
        targetDirection = directionToLastPostion;

        if (Vector2.Distance(transform.position, lastKnownPlayerPosition.Value) < 0.5f)
        {
            lastKnownPlayerPosition = null;
            StartCoroutine(PauseBeforeNextAction());
        }
    }

    private void HandleRandomDirectionChange()
    {
        if (!enableRandomDirections) return;
        
        changeDirectionCooldown -= Time.deltaTime;
        
        if (changeDirectionCooldown <= 0)
        {
            SetRandomDirection();
            changeDirectionCooldown = randomDirectionChangeTime;
        }
    }
    
    private void SetRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        targetDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad), 
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        ).normalized;
    }

    private void HandleObstacles()
    {
        obstacleAvoidanceCooldown -= Time.deltaTime;
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(obstacleLayerMask);

        float castDistance = obstacleCheckDistance;
        Vector2 castDirection = transform.up;

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position, 
            obstacleCheckCircleRadius, 
            transform.up, 
            contactFilter, 
            obstacleCollisions,obstacleCheckDistance
            );

        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollide = obstacleCollisions[index];

            if (obstacleCollide.collider.gameObject == gameObject) continue;
            
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
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
                );

            targetDirection = rotation * Vector2.up;
            break;
        }
    }
    
    private IEnumerator PauseBeforeNextAction()
    {
        StopMovement();
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        SetRandomDirection();
    }
    
    private void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
    }
    
    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
            );
        
        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
            _rigidbody.velocity = transform.up * speed;
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, obstacleCheckCircleRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, obstacleCheckDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)targetDirection * 2f);*/
        
        if (lastKnownPlayerPosition.HasValue)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(lastKnownPlayerPosition.Value, 0.3f);
        }
    }
}
