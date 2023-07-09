using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{
    private Animator animator;

    private readonly int horizontalSpeed = Animator.StringToHash("HorizontalSpeed");
    private readonly int jumping = Animator.StringToHash("Jumping");
    private readonly int isFalling = Animator.StringToHash("IsFalling");
    private readonly int landing = Animator.StringToHash("Landing");
    private readonly int dying = Animator.StringToHash("Dying");

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
    
    public void SetFalling(bool value)
    {
        animator.SetBool(isFalling, value);
    }
    
    public void Landing()
    {
        animator.SetTrigger(landing);
    }

    public void Dying()
    {
        animator.SetTrigger(dying);
    }
}
