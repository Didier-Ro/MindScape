using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private TriggerController currentTrigger;
    public GameObject dashText;
    public GameObject jumpText;
    public float dashSpeed = 10f;
    public float dashCooldownFrames = 60f; // Número de frames de enfriamiento
    public float dashStaminaCost = 25f;
    public float wallCheckDistance = 0.1f;
    private int lastDashFrame = 0; // Último frame en que se realizó un dash
    private Vector2 dashDirection;
    private bool isDashing = false;

    private Movement movementScript;
    private StaminaBar staminaBar;

    private bool isControllable = true;

    private void Start()
    {
        movementScript = GetComponent<Movement>();
        staminaBar = FindObjectOfType<StaminaBar>();
    }

    private void Update()
    {
        if (InputManager.GetInstance().DashInput() && Time.frameCount > lastDashFrame + dashCooldownFrames)
        {
            if (!IsTouchingWall())
            {
                AttemptDash();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        Vector2 dashMovement = new Vector2(dashDirection.x * dashSpeed * Time.fixedDeltaTime, dashDirection.y * dashSpeed * Time.fixedDeltaTime);
        movementScript.Rb.MovePosition(movementScript.Rb.position + dashMovement);

        // Verificar si el dash ha terminado
        if (Time.frameCount >= lastDashFrame + 1) // Se realiza una iteración del dash por frame
        {
            isDashing = false;
            movementScript.isMoving = true;

            // Activar los controles
            isControllable = true;

            if (currentTrigger != null)
            {
                currentTrigger.DisableTrigger();
            }

            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        }
    }
    public void SetCurrentTrigger(TriggerController trigger)
    {
        currentTrigger = trigger;
    }

    private void AttemptDash()
    {
        if (staminaBar != null && staminaBar.CurrentStamina >= dashStaminaCost)
        {
            float inputX = InputManager.GetInstance().MovementInput().x;
            float inputY = InputManager.GetInstance().MovementInput().y;
            if (inputX != 0 || inputY != 0)
            {
                // Verifica si está pasando por un agujero
                bool isPassingHole = IsPassingOverHole();

                dashDirection = new Vector2(inputX, inputY).normalized;
                isDashing = true;
                movementScript.isMoving = false;
                lastDashFrame = Time.frameCount;
                staminaBar.UseStamina(dashStaminaCost);
                PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DASHING);

                // Desactivar los controles
                isControllable = false;

                if (isPassingHole)
                {
                    StartCoroutine(JumpAnimation());
                }
            }
        }
    }

    private bool IsTouchingWall()
    {
        int wallLayerMask = LayerMask.GetMask("Wall");

        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayerMask);

        return (hitLeft.collider != null || hitRight.collider != null);
    }

    private bool IsPassingOverHole()
    {
        float radius = 1.0f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Hole"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator JumpAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 jumpScale = originalScale * 1.4f;
        transform.localScale = jumpScale;
        float jumpDuration = 0.1f;
        yield return new WaitForSeconds(jumpDuration);
        transform.localScale = originalScale;
    }
}
