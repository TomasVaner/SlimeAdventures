using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D coll;
    [SerializeField] private float groundDistance;
    [SerializeField] private Vector2 groundBox;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    private Vector2 speed;
    private Vector2 movement;
    private bool jumpPressed = false;
    private bool jumping = false;
    private float jumpHeldStartTime;
    private float jumpStartTime;
    private float jumpHeldDuration;
    [SerializeField] private LayerMask ground;
    
    [SerializeField] private bool isLookingRight;

    private bool grounded;

    private SpriteAnimationController anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        anim = GetComponent<SpriteAnimationController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
            Debug.Log(movement);
        }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpHeldStartTime = Time.time;
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpHeldDuration = Time.time - jumpHeldStartTime;
            jumpPressed = false;
        }
    }

    private void FixedUpdate()
    {
        bool old_grounded = grounded;
        grounded = IsGrounded();
        if (grounded && !old_grounded)
        {
            anim.Landing();
        }

        anim.SetFalling(!grounded);
        
        var velocity = rb.velocity;
        
        if (velocity.y < -0.01f)
            Debug.Log(Time.time + ": started falling");
        if (movement.SqrMagnitude() < 0.01f)
        {
            velocity.x += -velocity.x * acceleration * Time.deltaTime;
            if (Mathf.Abs(velocity.x) < 0.01f)
                velocity.x = 0;
        }
        else
        {
            velocity.x += acceleration * movement.x * Time.deltaTime;
            if (Mathf.Abs(velocity.x) > maxSpeed)
                velocity.x = maxSpeed * Mathf.Sign(velocity.x);
        }

        if (velocity.x > 0.01f && !isLookingRight
            || velocity.x < -0.01f && isLookingRight)
        {
            Flip();
        }
        rb.velocity = velocity;

        if (jumpPressed)
        {
            if (grounded && !jumping)
            {
                Debug.Log("Initializing jump");
                anim.Jump();
                jumping = true;
            }
        }
        else if (jumping)
        {
            AbortJump();
        }

        anim.SetHorizontalSpeed(Mathf.Abs(velocity.x)/maxSpeed);
        speed = rb.velocity;
    }

    private void ActivateJump()
    {
        if (grounded)
        {
            jumpStartTime = Time.time;
            Debug.Log(Time.time + ": Jump");
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2.0f * rb.gravityScale * jumpHeight));
            if (!jumpPressed)
                AbortJump();
        }
        else
        {
            jumping = false;
        }
    }

    private void AbortJump()
    {
        if (jumpStartTime <= 0.01f
            || Time.time < jumpStartTime + jumpHeldDuration)
            return;
        Debug.Log(Time.time + ": Aborting jump, jumpStartTime: " + jumpStartTime + " jumpHeldDuration: " + jumpHeldDuration);
        var velocity = rb.velocity;
        if (velocity.y > 0)
        {
            Debug.Log(Time.time + ": Resetting vertical speed");
            velocity.y = 0;
            rb.velocity = velocity;
        }

        jumping = false;
        jumpStartTime = 0f;
    }

    private void Flip()
    {
        var localScale = transform.localScale;
        localScale.x *= -1.0f;
        transform.localScale = localScale;
        isLookingRight = !isLookingRight;
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundBox, 0, Vector2.down, groundDistance, ground);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + groundDistance * Vector3.down, groundBox);
    }
}
