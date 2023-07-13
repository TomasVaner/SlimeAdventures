/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using UnityEngine;
using UnityEngine.Events;

namespace Controller
{
    [RequireComponent(typeof(Animator))]
    [ExecuteInEditMode]
    public class SpriteAnimationController : MonoBehaviour
    {
        private Animator animator;

        [SerializeField] private UnityEvent jumpComplete;
        [SerializeField] private UnityEvent dyingComplete;

        private readonly int horizontalSpeed = Animator.StringToHash("HorizontalSpeed");
        private readonly int jumping = Animator.StringToHash("Jumping");
        private readonly int isFalling = Animator.StringToHash("IsFalling");
        private readonly int dying = Animator.StringToHash("IsDying");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetHorizontalSpeed(float speed)
        {
            animator.SetFloat(horizontalSpeed, speed);
        }

        public void Jump()
        {
            animator.SetTrigger(jumping);
        }

        private void JumpComplete()
        {
            jumpComplete.Invoke();
        }

        public void SetFalling(bool value)
        {
            animator.SetBool(isFalling, value);
        }

        public void SetDying(bool value)
        {
            animator.SetBool(dying, value);
        }
    
        private void DyingComplete()
        {
            dyingComplete.Invoke();
        }
    }
}
