using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashController : MonoBehaviour
{
    
    #region Singletone
    private static DashController Instance;
    public static DashController GetInstance() 
    { 
        return Instance;
    }
    #endregion

    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private InputManager inputManager;
    private bool isDashing = false;
    private bool dashUnlocked = true; // Added to control dash unlock
    private float dashTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = InputManager.GetInstance();
    }

    private void Update()
    {
        if (dashUnlocked && !isDashing && inputManager.DashInput())
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        dashUnlocked = false; // Lock dash while dashing
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = transform.right * dashSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Start cooldown timer
        yield return new WaitForSeconds(dashCooldown);

        // Unlock dash after cooldown
        dashUnlocked = true;
    }

    // Method to unlock dash externally, for example, after a certain event or time
    public void UnlockDash()
    {
        dashUnlocked = true;
    }
}