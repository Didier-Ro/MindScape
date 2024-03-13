using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    [SerializeField] private bool canInteract = false;
    [SerializeField] private bool isInteracting = false;

    [SerializeField] private GameObject interactiveObject;
    
    private Vector2 moveInputValue = Vector2.zero;
    private GAME_STATE currentGamestate = default;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        DialogManager.GetInstance().OnCloseDialog += () =>
        {
            if (currentGamestate == GAME_STATE.READING)
            {
                interactiveObject.GetComponent<Istepable>().Deactivate();
            }
        };
    }

    void Update()
    {
        if (currentGamestate == GAME_STATE.READING)
        {
            DialogManager.GetInstance().HandleUpdate();
        }
        SetInteraction();
        Debug.Log(currentGamestate);
        // Mover el jugador
        //MovePlayer();

        /*  if (Input.GetKeyDown(KeyCode.F) && canInteract)
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
          }*/
    }

    public void SetInteraction()
    {
        if (canInteract && InputManager.GetInstance().InteractInput())
        {
            interactiveObject.GetComponent<Istepable>().Activate();
            currentGamestate = GameManager.GetInstance().GetCurrentGameState();
            canInteract = false;
            isInteracting = true;
        }
        
    }

    private void FixedUpdate()
    {
        MovePlayer();

       /* if (Input.GetKeyDown(KeyCode.F) && canInteract)
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
        }*/
    }

    void MovePlayer()
    {
        moveInputValue = InputManager.GetInstance().MovementInput();
       
        Vector2 movement = new Vector2(moveInputValue.x, moveInputValue.y);

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