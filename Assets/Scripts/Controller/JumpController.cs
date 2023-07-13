/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using BaseClasses;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [ExecuteInEditMode]
    public class JumpController : MonoBehaviour
    {
    #region Variables
        [Header("Ground Check")]
        [SerializeField] private float groundDistance;
        [SerializeField] private Vector2 groundBox;
        [SerializeField] private LayerMask solidGround;
        [SerializeField] private LayerMask jumpthroughGround;
        [Header("Jump Parameters")]
        [SerializeField] private float jumpHeight;

        [SerializeField] private float maxFallVelocity;
    #endregion

    #region Events
    #endregion
        
    #region Properties

        public bool JumpThroughPlatforms
        {
            get => _jumpThroughPlatforms;
            set
            {
                if (_jumpThroughPlatforms == value)
                    return;
                _jumpThroughPlatforms = value;
                if (_jumpThroughPlatforms)
                    _collider2D.forceReceiveLayers &= ~jumpthroughGround;
                else if (!_collider2D.IsTouchingLayers(jumpthroughGround))
                    _collider2D.forceReceiveLayers |= jumpthroughGround;
            }
        }
        
    #endregion
        
    #region Private fields
        private Rigidbody2D rb;
        private Collider2D _collider2D;
        private bool _jumpThroughPlatforms = false;
    #endregion

    #region Unity Methods

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            var velocity = rb.velocity;
            if (velocity.y < -maxFallVelocity)
            {
                velocity.y = -maxFallVelocity;
                rb.velocity = velocity;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!_jumpThroughPlatforms
                && ((1 << other.gameObject.layer) & jumpthroughGround) != 0)
                _collider2D.forceReceiveLayers |= jumpthroughGround;
        }

    #endregion
    
    #region Public Methods

        public void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2.0f * rb.gravityScale * jumpHeight));
        }
        
        public void AbortJump()
        {
            var velocity = rb.velocity;
            if (velocity.y > 0)
            {
                velocity.y = 0;
                rb.velocity = velocity;
            }
        }
        
        public bool IsGrounded()
        {
            return rb.velocity.y < 0.01f && Physics2D.BoxCast(transform.position, groundBox, 0, Vector2.down, groundDistance, solidGround | (_jumpThroughPlatforms ? 0 : jumpthroughGround));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireCube(transform.position + groundDistance * Vector3.down, groundBox);
        }
    #endregion

    #region PrivateMethods
    #endregion
    }
}