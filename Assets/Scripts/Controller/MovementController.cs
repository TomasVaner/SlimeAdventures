/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(SpriteAnimationController))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
    #region Variables

        [SerializeField] private float acceleration;
        [SerializeField] private float maxHorizontalVelocity;
        [SerializeField] private bool isLookingRight;

    #endregion

    #region Private Fields

        private Rigidbody2D rb;
        private SpriteAnimationController anim;
        private bool _canMove;
    #endregion

    #region Properties

        public Vector2 Movement { get; set; }

        public bool CanMove
        {
            get => _canMove;
            set
            {
                _canMove = value;
                if (!_canMove)
                    Movement = Vector2.zero;
            }
        }
        

    #endregion

    #region Unity Functions

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<SpriteAnimationController>();
        }

        private void FixedUpdate()
        {
            var velocity = rb.velocity;
            if (!CanMove
                || Math.Abs(Movement.x) < 0.01f
                || (Math.Sign(Movement.x) != Math.Sign(velocity.x) && Math.Abs(velocity.x) > Math.Abs(Movement.x)))
            {
                // coming to a stop
                velocity.x += -velocity.x * acceleration * Time.deltaTime;
                if (Mathf.Abs(velocity.x) < 0.01f)
                    velocity.x = 0;
            }

            if (CanMove && Math.Abs(Movement.x) > 0.01f)
            {
                // accelerating
                velocity.x += acceleration * Movement.x * Time.deltaTime;
                if (Mathf.Abs(velocity.x) > maxHorizontalVelocity)
                    velocity.x = maxHorizontalVelocity * Mathf.Sign(velocity.x);
            }

            if (velocity.x > 0.01f && !isLookingRight
                || velocity.x < -0.01f && isLookingRight)
            {
                Flip();
            }

            rb.velocity = velocity;

            anim.SetHorizontalSpeed(Mathf.Abs(velocity.x) / maxHorizontalVelocity);
        }

    #endregion

    #region Private Methods

        private void Flip()
        {
            var localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isLookingRight = !isLookingRight;
        }

    #endregion
    }
}