using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;

    private float lastDashTime = 0f;
    private Vector2 dashDirection;
    private bool isDashing = false;

    private Movement movementScript;
    private StaminaBar staminaBar;

    private void Start()
    {
        movementScript = GetComponent<Movement>();
        staminaBar = FindObjectOfType<StaminaBar>();
    }

    private void Update()
    {
        if (InputManager.GetInstance().DashInput() && Time.time > lastDashTime + dashCooldown)
        {
            AttemptDash();
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        Collider2D feetCollider = FindFeetCollider();
        if (feetCollider != null)
        {
            feetCollider.enabled = false;
        }

        movementScript.isMoving = false;
        float dashTimer = 0f;

        while (dashTimer < dashTime)
        {
            Vector2 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
            movementScript.Rb.MovePosition(movementScript.Rb.position + dashMovement);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        if (feetCollider != null)
        {
            feetCollider.enabled = true;
        }

        movementScript.isMoving = true;
        isDashing = false;
        lastDashTime = Time.time;
    }

    private void AttemptDash()
    {
        if (staminaBar != null && staminaBar.CurrentStamina >= dashStaminaCost)
        {
            float inputX = InputManager.GetInstance().MovementInput().x;
            float inputY = InputManager.GetInstance().MovementInput().y;
            if (inputX != 0 || inputY != 0)
            {
                dashDirection = new Vector2(inputX, inputY).normalized;
                StartCoroutine(PerformDash());
                staminaBar.UseStamina(dashStaminaCost);
            }
        }
    }

    private Collider2D FindFeetCollider()
    {
        GameObject[] feetObjects = GameObject.FindGameObjectsWithTag("Feet");
        if (feetObjects.Length > 0)
        {
            return feetObjects[0].GetComponent<Collider2D>();
        }
        return null;
    }
}