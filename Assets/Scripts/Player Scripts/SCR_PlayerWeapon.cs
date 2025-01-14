using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float delayTime = 0.3f;
    [SerializeField] private int DamageOnHit = 1;
    [SerializeField] private GameObject AttackPrefab;
    
    [SerializeField] private List<Transform> CircleOrigins = new List<Transform>();
    [SerializeField] private List<float> Radii = new List<float>();

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    private bool attackLock;
    public Vector2 PointerPosition { get; set; }
    public bool IsAttacking { get; private set; }
    public InputActionAsset AttackAction;
    public Animator Animator;

    public delegate void OnAttack(Vector2 attackPosition, Vector2 attackDirection);

    public event OnAttack AttackEvent;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delayTime);
        attackLock = false;
    }

    public void AttackMethod()
    {
        if (attackLock)
        {
            return;
        }

        Animator.SetTrigger("Attack");
        IsAttacking = true;

        Vector2 attackPosition = transform.position;
        Vector2 attackDirection = (PointerPosition - (Vector2)transform.position).normalized;
        AttackEvent?.Invoke(attackPosition, attackDirection);

        Instantiate(AttackPrefab, transform.position, Quaternion.identity);

        StartCoroutine(DetectCollidersAfterDelay());
        attackLock = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DetectCollidersAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        DetectColliders();
    }

    private void Update()
    {
        if (IsAttacking)
        {
            return;
        }

        // Update the pointer position
        PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction to the pointer
        Vector2 direction = PointerPosition - (Vector2)transform.position;

        // Calculate the angle between the weapon and the pointer
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the weapon to face the pointer
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        if (CircleOrigins.Count == Radii.Count)
        {
            for (int i = 0; i < CircleOrigins.Count; i++)
            {
                if (CircleOrigins[i] != null)
                {
                    Gizmos.DrawWireSphere(CircleOrigins[i].position, Radii[i]);
                }
            }
        }
        else
        {
            Debug.LogWarning("Mismatched CircleOrigins and Radii count in SCR_PlayerWeapon!");
        }
    }

    public void DetectColliders()
    {
        if (CircleOrigins.Count != Radii.Count)
        {
            Debug.LogWarning("Cannot detect colliders: Mismatched CircleOrigins and Radii!");
            return;
        }
        
        for (int i = 0; i < CircleOrigins.Count; i++)
        {
            Transform origin = CircleOrigins[i];
            float radius = Radii[i];

            if (origin == null) continue;

            foreach (Collider2D collider in Physics2D.OverlapCircleAll(origin.position, radius))
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") &&
                    !hitEnemies.Contains(collider.gameObject))
                {
                    hitEnemies.Add(collider.gameObject);
                    
                    Vector2 attackDirection = (collider.transform.position - transform.position).normalized;

                    collider.gameObject.GetComponent<SCR_EnemyHealth>()?.TakeDamage(
                        DamageOnHit,
                        transform.position,
                        attackDirection
                    );

                    SCR_KnockbackFeedBack knockbackComponent =
                        collider.gameObject.GetComponent<SCR_KnockbackFeedBack>();
                    knockbackComponent?.PlayFeedback(gameObject);
                }
            }
        }

        StartCoroutine(ClearHitEnemiesAfterDelay());
    }

    private IEnumerator ClearHitEnemiesAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        hitEnemies.Clear();
    }
}