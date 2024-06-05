using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashController : MonoBehaviour
{
    private TriggerController currentTrigger;
    public GameObject dashText;
    public GameObject jumpText;
    public float dashSpeed = 10f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;
    public float wallCheckDistance = 0.1f;
    private float lastDashTime = 0f;
    private Vector2 dashDirection;
    private GAME_STATE currentGamestate = default;
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
            if (!IsTouchingWall() && CanPerformDash())
            {
                AttemptDash();
            }
        }
    }

    private void FixedUpdate()
    {

    }

    private IEnumerator PerformDash(bool isPassingHole)
    {
        isDashing = true;
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DASHING);

        movementScript.isMoving = false;
        float dashTimer = 0f;

        if (isPassingHole)
        {
            StartCoroutine(JumpAnimation());
        }

        while (dashTimer < dashTime)
        {
            Vector2 dashMovement = new Vector2(dashDirection.x * dashSpeed * Time.deltaTime, dashDirection.y * dashSpeed * Time.deltaTime);
            movementScript.Rb.MovePosition(movementScript.Rb.position + dashMovement);
            dashTimer += Time.deltaTime;
            yield return null;
        }

        movementScript.isMoving = true;
        isDashing = false;
        lastDashTime = Time.time;
        if (currentTrigger != null)
        {
            currentTrigger.DisableTrigger();
        }

        if (isPassingHole)
        {
            yield return new WaitForSeconds(0.5f);
        }
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
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
                bool isPassingHole = IsPassingOverHole();

                dashDirection = new Vector2(inputX, inputY).normalized;
                StartCoroutine(PerformDash(isPassingHole));
                staminaBar.UseStamina(dashStaminaCost);
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

    private bool CanPerformDash()
    {
        // Verificar si al menos una de las dos imágenes de las botas tiene fillAmount mayor que cero
        foreach (Image image in staminaBar.dashImages)
        {
            if (image.fillAmount > 0)
            {
                return true;
            }
        }
        return false;
    }
}