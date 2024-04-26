using UnityEngine;
using UnityEngine.Serialization;

public class DashController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;

    [SerializeField] private GameObject feetCollider;

    private float lastDashTime;
    private Vector2 dashDirection;
    private bool isDashingOnCooldown = false;

    private Movement movementScript;
    private StaminaBar staminaBar;

    private void Start()
    {
        movementScript = GetComponent<Movement>();
        staminaBar = FindObjectOfType<StaminaBar>();
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
                staminaBar.UseStamina(dashStaminaCost); // Reduce la stamina
            }
        }
    }

    private System.Collections.IEnumerator PerformDash()
    {
        movementScript.isMoving = false;
        float dashTimer = 0f;

        while (dashTimer < dashTime)
        {
            feetCollider.SetActive(false);
            Vector2 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
            movementScript.Rb.MovePosition(movementScript.Rb.position + dashMovement);
            dashTimer += Time.deltaTime;
            yield return null;
        }
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DASHING);
        movementScript.isMoving = true;
        yield return new WaitForSeconds(dashCooldown);
        feetCollider.SetActive(true);
        isDashingOnCooldown = false; // Restablece el cooldown
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);

    }
}