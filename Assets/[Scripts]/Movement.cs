using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    public Vector2 input;
    public bool isMoving = true;
    [SerializeField] private int levelConditionCheck = 4;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] bool canInteract = false;
    [SerializeField] private bool isInteracting = false;
    [SerializeField] GameObject interactiveObject;
    private bool isSuscribed = true;
    private GAME_STATE currentGamestate = default;
    Animator animator;
    Rigidbody2D rb;

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        Debug.Log(_newGameState.ToString());
       isMoving = _newGameState == GAME_STATE.EXPLORATION;
    }

    #endregion
    

    private void Start()
    {
        SubscribeToGameManagerGameState();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        DialogManager.GetInstance().OnCloseDialog += () =>
        {
            if (currentGamestate == GAME_STATE.READING)
            {
                interactiveObject.GetComponent<Istepable>().Deactivate();
                canInteract = true;
            }
        };
        if (GameManager.GetInstance().IsConditionCompleted(levelConditionCheck))
        {
            transform.position = CheckpointManager.FindNearestCheckpoint(transform.position);
          
        }
        else
        {
            GameManager.GetInstance().MarkConditionCompleted(levelConditionCheck);
            transform.position = initialPosition;
            GameManager.GetInstance().SavePlayerPosition(initialPosition);
        }
    }

    void FixedUpdate()
    {

        input = InputManager.GetInstance().MovementInput();
        HandleMovementInput();
        if (currentGamestate == GAME_STATE.READING)
        {
            DialogManager.GetInstance().HandleUpdate();
            isMoving = false;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stepable"))
        {
            interactiveObject = other.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stepable"))
        {
            interactiveObject = null;
            canInteract = false;
            isInteracting = false;
        }
    }

    void HandleMovementInput()
    {
        if (isMoving)
        {
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
            Vector2 movement = input.normalized * walkSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}

