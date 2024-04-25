using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;

    private float lastDashTime;
    private Vector2 dashDirection;
    private bool isDashingOnCooldown = false;

    private Movement movementScript;
    private StaminaBar staminaBar;

    private void Start()
    {
        movementScript = GetComponent<Movement>();
        // Buscar StaminaBar en la escena y asignar su referencia
        staminaBar = FindObjectOfType<StaminaBar> ();
    }

    void Update()
    {
        if (InputManager.GetInstance().DashInput() && Time.time > lastDashTime + dashCooldown)
        {
            AttemptDash();
        }
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
                lastDashTime = Time.time;
                isDashingOnCooldown = true;
                staminaBar.UseStamina(dashStaminaCost);
            }
        }
    }

    private IEnumerator PerformDash()
    {
        movementScript.isMoving = false;
        float dashTimer = 0f;

        while (dashTimer < dashTime)
        {
            Vector2 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
            movementScript.Rb.MovePosition(movementScript.Rb.position + dashMovement);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        movementScript.isMoving = true;
        yield return new WaitForSeconds(dashCooldown);
        isDashingOnCooldown = false;
    }
}