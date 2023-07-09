using System;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    [RequireComponent(typeof(SpriteAnimationController))]
    public class Creature : MonoBehaviour
    {
        private Rigidbody2D rb;
        private CircleCollider2D coll;
        
        [SerializeField] private bool isLookingRight;
        
        [Header("Movement")]
        [SerializeField] private float groundDistance;
        [SerializeField] private Vector2 groundBox;

        [SerializeField] private float jumpHeight;
        [SerializeField] private float acceleration;
        [SerializeField] private float maxSpeed;
        [SerializeField] private LayerMask ground;
        private bool grounded;
        private bool enableMovement = false;
        protected Vector2 movement;
        private bool jumpActive = false;
        private bool jumping = false;
        private float jumpHeldStartTime;
        private float jumpStartTime;
        private float jumpHeldDuration;
        private SpriteAnimationController anim;
        [SerializeField] private Transform spawnPosition;
        [Header("Health")]
        [SerializeField] private Func<float> getMaxHealth;

        private float currentHealth;
        [SerializeField] private float immunityTime;
        private float damagedTime;
        [SerializeField] private float knockbackResistance;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<CircleCollider2D>();
            anim = GetComponent<SpriteAnimationController>();
        }
        
        private void Start()
        {
            Respawn();
        }
        
        private void Respawn()
        {
            transform.position = spawnPosition.position;
            anim.SetDying(false);
            AbortJump();
            enableMovement = true;
        }

        private void FixedUpdate()
        {
            if (!enableMovement)
                return;
            bool old_grounded = grounded;
            grounded = IsGrounded();
            if (grounded && !old_grounded)
            {
                anim.Landing();
            }

            anim.SetFalling(!grounded);

            var velocity = rb.velocity;

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

            if (jumpActive)
            {
                if (grounded && !jumping)
                {
                    anim.Jump();
                    jumping = true;
                }
            }
            else if (jumping)
            {
                AbortJump();
            }
    
            anim.SetHorizontalSpeed(Mathf.Abs(velocity.x)/maxSpeed);
        }

        protected void StartJump()
        {
            jumpHeldStartTime = Time.time;
            jumpActive = true;
        }

        protected void EndJump()
        {
            jumpHeldDuration = Time.time - jumpHeldStartTime;
            jumpActive = false;
        }
        
        private void AbortJump()
        {
            if (jumpStartTime <= 0.01f
                || Time.time < jumpStartTime + jumpHeldDuration)
                return;
            var velocity = rb.velocity;
            if (velocity.y > 0)
            {
                velocity.y = 0;
                rb.velocity = velocity;
            }

            jumping = false;
            jumpStartTime = 0f;
        }
        
        protected bool IsGrounded()
        {
            return Physics2D.BoxCast(transform.position, groundBox, 0, Vector2.down, groundDistance, ground);
        }
        
        private void Flip()
        {
            var localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;
            isLookingRight = !isLookingRight;
        }
        
        public void Damage(float value, Transform source, float knockbackForce = 10)
        {
            if (damagedTime + immunityTime < Time.time)
                return;
            damagedTime = Time.time;
            currentHealth -= value;
            if (currentHealth < 0f)
                Die();
            rb.velocity = (source.transform.position - transform.position).normalized *
                          (knockbackForce / knockbackResistance);
        }
        
        private void Die()
        {
            anim.SetDying(true);
            enableMovement = false;
        }

        private void Died()
        {
            Respawn();
        }
        
        private void ActivateJump()
        {
            if (grounded)
            {
                jumpStartTime = Time.time;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2.0f * rb.gravityScale * jumpHeight));
                if (!jumpActive)
                    AbortJump();
            }
            else
            {
                jumping = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + groundDistance * Vector3.down, groundBox);
        }
    }
}
