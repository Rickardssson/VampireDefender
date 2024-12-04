using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SCR_PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float delayTime = 0.3f;
    [SerializeField] private float radius;
    [SerializeField] private int DamageOnHit = 1;

    private bool attackLock;
    
    public Vector2 PointerPosition { get; set; }
    public bool IsAttacking { get; private set; }
    public InputActionAsset AttackAction;
    public Animator Animator;
    public Transform CircleOrigin;
    
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
        StartCoroutine(DetectCollidersAfterDelay());
        attackLock = true;     
        StartCoroutine(DelayAttack());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
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
        Vector3 position = CircleOrigin == null ? Vector3.zero : CircleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleOrigin.position, radius))
        {
            Debug.Log(collider.name);
            
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Take damage on hit
                collider.gameObject.GetComponent<SCR_EnemyHealth>().TakeDamage(DamageOnHit);
                
                // Apply knockback
                SCR_KnockbackFeedBack knockbackComponent = 
                    collider.gameObject.GetComponent<SCR_KnockbackFeedBack>();
                if (knockbackComponent != null)
                {
                    knockbackComponent.PlayFeedback(gameObject);
                }
            }
        }
    }
}