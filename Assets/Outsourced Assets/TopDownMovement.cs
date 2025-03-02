using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

            /////////////// INFORMATION ///////////////
// This script automatically adds a Rigidbody2D and a CapsuleCollider2D component in the inspector.
// The following components are needed: Player Input

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class TopDownMovement : MonoBehaviour
{
    public float CurrentDashCooldown => dashCooldown;
    
    public float maxSpeed = 7;
    public float dashSpeed = 40;
    public bool isDashing = false;
    public float dashDuration = 0.1f;
    public float dashCooldownDuration = 1f;
    public float VoiceLineCooldownDuration = 1;
    
    public bool controlEnabled { get; set; } = true;    // You can edit this variable from Unity Events
    public UnityEvent onAction1, onAction2;

    public AudioSource dashSound;
    public AudioSource voiceLineSound;
    
    private float voideCooldown = 0f;
    private float dashTimer;
    private float dashCooldown = 0f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    /*private CinemachineVirtualCamera _virtualCamera;*/
    private Camera _camera;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Set gravity scale to 0 so player won't "fall" 
        rb.gravityScale = 0;
        _camera = Camera.main;
        /*_virtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();*/
        
        if (dashSound == null)
        {
            Debug.LogError("No dash sound assigned.");
        }
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < 0 && rb.velocity.x < 0) || 
            (screenPosition.x > _camera.pixelWidth && rb.velocity.x > 0))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        if ((screenPosition.y < 0 && rb.velocity.y < 0) || 
            (screenPosition.y > _camera.pixelHeight && rb.velocity.y > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = moveInput.normalized * dashSpeed;
            dashTimer -= Time.fixedDeltaTime;
            
            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashCooldown = dashCooldownDuration;
            }
        }
        // Set velocity based on direction of input and maxSpeed
        else if (controlEnabled && !isDashing)
        {
            rb.velocity = moveInput.normalized*maxSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        
        if (dashCooldown > 0f)
        {
            dashCooldown -= Time.fixedDeltaTime;
        }

        if (voideCooldown > 0f)
        {
            voideCooldown -= Time.fixedDeltaTime;
        }
        
        // Prevent player from going offscreen (if camera is not moving)
        PreventPlayerGoingOffScreen();
        // Write code for walking animation here. (Suggestion: send your current velocity into the Animator for both the x- and y-axis.)
    }
    
    // Handle Move-input
    // This method can be triggered through the UnityEvent in PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing && dashCooldown <= 0f && moveInput.sqrMagnitude > 0)
        {
            isDashing = true;
            dashTimer = dashDuration;
            
            dashCooldown = dashCooldownDuration;
            
            if (dashSound != null && !dashSound.isPlaying)
            {
                dashSound.Play();
            }

            if (voiceLineSound != null && voideCooldown <= 0f)
            {
                voiceLineSound.Play();
                voideCooldown = VoiceLineCooldownDuration;
            }
        }
    }

    public void Action1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAction1.Invoke();
        }
    }

    public void Action2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAction2.Invoke();
        }
    }
}
