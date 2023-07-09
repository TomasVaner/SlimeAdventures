using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : Creature
{
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
        }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartJump();
        }
        else if (context.canceled)
        {
            EndJump();
        }
    }
}
