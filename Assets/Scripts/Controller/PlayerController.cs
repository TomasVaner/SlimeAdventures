/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using System.ComponentModel;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    [RequireComponent(typeof(SpriteAnimationController))]
    [RequireComponent(typeof(JumpController))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(Creature))]
    [RequireComponent(typeof(PlayerAudioController))]
    public class PlayerController : MonoBehaviour
    {
    #region Variables
        [SerializeField] private int jumpsMax = 1;
    #endregion

    #region Private Fields
        private SpriteAnimationController anim;
        private JumpController jump;
        private MovementController move;
        private Creature creature;
        private PlayerAudioController _audio;
        
        private int jumpsLeft;
        private float jumpHeldStartTime;
        private float jumpStartTime;
        private float jumpHeldDuration;
    #endregion

    #region Unity Functions

        private void Awake()
        {
            anim = GetComponent<SpriteAnimationController>();
            jump = GetComponent<JumpController>();
            jumpsLeft = jumpsMax;
            move = GetComponent<MovementController>();
            creature = GetComponent<Creature>();
            creature.PropertyChanged += CreatureOnPropertyChanged;
            _audio = GetComponent<PlayerAudioController>();
        }

        private void FixedUpdate()
        {
            if (jumpStartTime > 0f
                && jumpHeldDuration > 0f
                && Time.time > jumpStartTime + jumpHeldDuration)
            {
                jump.AbortJump();
            }

            bool grounded = jump.IsGrounded();
            if (grounded)
            {
                jumpStartTime = 0f;
                jumpsLeft = jumpsMax;
            }
            anim.SetFalling(!grounded);
        }

    #endregion
        
    #region InputCallbacks

        public void MovementInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!creature.CanMove)
                    return;
                move.Movement = context.ReadValue<Vector2>();
                jump.JumpThroughPlatforms = move.Movement.y < -0.01f;
            }

            if (context.canceled)
            {
                move.Movement = new Vector2();
                jump.JumpThroughPlatforms = false;
            }
        }
        
        public void JumpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (jump.IsGrounded())
                {
                    anim.Jump();
                    jumpStartTime = 0f;
                }
                else if (jumpsLeft > 0)
                {
                    _audio.PlayJump();
                    jump.Jump();
                    jumpStartTime = Time.time;
                    --jumpsLeft;
                }
                jumpHeldStartTime = Time.time;
                jumpHeldDuration = 0f;
            }
            else if (context.canceled)
            {
                jumpHeldDuration = Time.time - jumpHeldStartTime;
            }
        }
    #endregion

    #region SpriteAnimationController Callbacks

        public void JumpAnimationCompleteCallback()
        {
            if (!creature.CanMove)
                return;
            if (jump.IsGrounded())
            {
                _audio.PlayJump();
                jumpStartTime = Time.time;
                jump.Jump();
                --jumpsLeft;
            }
        }

    #endregion

    #region Creature Callbacks

        private void CreatureOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(creature.CanMove))
            {
                jump.CanMove = creature.CanMove;
                move.CanMove = creature.CanMove;
            }
        }
        
    #endregion
        
    }
}
