using System.Collections;
using UnityEngine;

public class DashController : MonoBehaviour
{
    public float dashSpeed = 10f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 25f;
    public float wallCheckDistance = 0.1f;
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
            if (!IsTouchingWall())
            {
                AttemptDash();
            }
        }
    }

    private IEnumerator PerformDash(bool isJumping)
    {
        isDashing = true;
        Collider2D feetCollider = FindFeetCollider();
        if (feetCollider != null)
        {
            feetCollider.enabled = false;
        }

        movementScript.isMoving = false;
        float dashTimer = 0f;

        // Verificar si se debe ejecutar la animación de salto
        if (isJumping)
        {
            StartCoroutine(JumpAnimation());
        }

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


    private IEnumerator JumpAnimation()
    {
        // Escala original del jugador
        Vector3 originalScale = transform.localScale;

        // Escala aumentada para simular el "salto"
        Vector3 jumpScale = originalScale * 1.4f;

        // Aplicar la escala aumentada temporalmente
        transform.localScale = jumpScale;

        // Duración de la animación de "salto"
        float jumpDuration = 0.1f;

        // Esperar el tiempo de la animación de "salto"
        yield return new WaitForSeconds(jumpDuration);

        // Restaurar la escala original del jugador después de la animación de "salto"
        transform.localScale = originalScale;
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
                bool isJumping = IsPassingOverHole();
                StartCoroutine(PerformDash(isJumping));
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

    private bool IsTouchingWall()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance);
        return hitLeft.collider != null || hitRight.collider != null;
    }

    private bool IsPassingOverHole()
    {
        // Definir el radio del círculo de detección alrededor del jugador
        float radius = 1.0f; // Ajusta este valor según sea necesario

        // Obtener todos los colliders de vacío dentro del círculo de detección
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        // Verificar si alguno de los colliders encontrados es un vacío
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Hole"))
            {
                // Si el jugador está cerca de un vacío, retornar verdadero para ejecutar la animación de salto
                return true;
            }
        }

        // Si no se encontraron vacíos cerca del jugador, retornar falso para evitar la animación de salto
        return false;
    }
}