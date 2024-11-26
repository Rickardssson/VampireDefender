using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCREnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    /*private Vector2 _targetDirection;*/
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();

        if (_playerAwarenessController == null)
        {
            Debug.LogError("PlayerAwareness Controller not found");
        }
    }
    
    private void FixedUpdate()
    {
        if (_playerAwarenessController && _playerAwarenessController.AwareOfPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMovement();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 directionToPlayer = _playerAwarenessController.DirectionToPlayer;
        _rigidbody.velocity = directionToPlayer * _speed;
    }

    private void StopMovement()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
