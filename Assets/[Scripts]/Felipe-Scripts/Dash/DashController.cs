using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public LayerMask dashLayerMask;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector2 lastMovementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") && canDash)
        {
            Vector2 dashDirection = DetermineDashDirection();
            StartDash(dashDirection);
        }
    }

    public void StartDash(Vector2 dashDirection)
    {
        if (!isDashing)
        {
            StartCoroutine(Dash(dashDirection));
        }
    }

    IEnumerator Dash(Vector2 dashDirection)
    {
        isDashing = true;
        canDash = false;

        Vector2 targetPosition = (Vector2)transform.position + (dashDirection * dashDistance);

        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, targetPosition, dashTime / dashDuration));
            dashTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(targetPosition);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private Vector2 DetermineDashDirection()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (inputDirection != Vector2.zero)
        {
            lastMovementDirection = inputDirection;
            return inputDirection;
        }
        else
        {
            return lastMovementDirection;
        }
    }
}
