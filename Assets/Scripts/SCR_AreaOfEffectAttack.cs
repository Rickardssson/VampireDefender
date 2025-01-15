using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SCR_AreaOfEffectAttack : MonoBehaviour
{
    [Header("References")]
    public UnityEngine.Events.UnityEvent OnAttackInitiated;
    [SerializeField] private GameObject aoeAttackAnimationPrefab;
    public Vector3 mousePosition;
    private Vector3 attackPosition;
    public int damageOnHit = 10;
    public float attackDelay = 1f;
    public float coolDown = 7f;
    public float _coolDown;
    private float _attackDelay;
    public bool playerIsAttacking;
    private bool hasSpawnedAnimation;
    public float attackRadius;
    public bool hasAttacked;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private CircleCollider2D circleCollider2D;

    private void Start()
    {
        OnAttackInitiated ??= new UnityEngine.Events.UnityEvent();
        playerIsAttacking = false;
        _attackDelay = attackDelay;
        _coolDown = coolDown;
        circleCollider2D = GetComponent<CircleCollider2D>();
        _coolDown = 0f;

    }

    void FixedUpdate()
    {
        _coolDown -= Time.deltaTime;
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (Input.GetKey(KeyCode.Q) && _coolDown <= 0)
        {
            playerIsAttacking = true;
            OnAttackInitiated.Invoke();
            transform.position = attackPosition;
            hasSpawnedAnimation = true;
        }

        if (hasAttacked)
        {
            _attackDelay -= Time.deltaTime;
            _coolDown = coolDown;
            if (hasSpawnedAnimation)
            {
                Instantiate(aoeAttackAnimationPrefab, mousePosition, quaternion.identity);
                hasSpawnedAnimation = false;
            }
        }
        if (_attackDelay <=0)
        {
            Attack();
            playerIsAttacking = false;
            _attackDelay = attackDelay;
        }
        attackPosition = mousePosition;
        hasAttacked = playerIsAttacking;
        
    }
    

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, 1 << LayerMask.NameToLayer("Enemy"));
        
        foreach (Collider2D en in enemies)
        {
            if (en.gameObject.layer == LayerMask.NameToLayer("Enemy") &&
                !hitEnemies.Contains(en.gameObject))
            {
                hitEnemies.Add(en.gameObject);

                // Take damage on hit
                Vector2 attackDirection = (en.transform.position - transform.position).normalized;

                en.gameObject.GetComponent<SCR_EnemyHealth>().TakeDamage(
                    damageOnHit,
                    transform.position,
                    attackDirection
                );

                // Apply knockback
                SCR_KnockbackFeedBack knockbackComponent =
                    en.gameObject.GetComponent<SCR_KnockbackFeedBack>();
                if (knockbackComponent != null)
                {
                    knockbackComponent.PlayFeedback(gameObject);
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (attackRadius <= 0) return;
        
        Gizmos.color = Color.yellow;
        Vector3 position = Application.isPlaying ? attackPosition : transform.position;
        
        position.z = 0;
        
        Gizmos.DrawWireSphere(position, attackRadius);
    
    }
    
}
