using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    [SerializeField] private bool canInteract = false;
    [SerializeField] private bool isInteracting = false;

    [SerializeField] private GameObject interactiveObject;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Mover el jugador
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.F) && canInteract)
        {
            interactiveObject.GetComponent<Istepable>().Activate();
            canInteract = false;
            isInteracting = true;
        }

        if (Input.GetKeyDown(KeyCode.B) && isInteracting)
        {
            interactiveObject.GetComponent<Istepable>().Deactivate();
            canInteract = true;
            isInteracting = false;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Stepable"))
        {
            interactiveObject = collision.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Stepable"))
        {
            interactiveObject = null;
            canInteract = false;
            isInteracting = false;
        }
    }
}