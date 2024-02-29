using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Mover el jugador
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        movement.Normalize(); // Evitar movimientos diagonales más rápidos

        rb.velocity = movement * moveSpeed;

        // Rotar el jugador hacia la dirección del movimiento
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}