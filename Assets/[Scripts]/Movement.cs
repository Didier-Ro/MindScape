using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = default;
    [SerializeField] private float moveDelay = default;
    [SerializeField] private float walkSpeed = default;
    private Direction currentDir = Direction.South;
    private Vector2 input;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private float progress;
    private float remainingMoveDelay = 0f;
    private int framesPerMove = 60;
    private bool isSuscribed = true;

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState() // Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState) // Analyze the Game State type and makes different behavior
    {
        isMoving = _newGameState == GAME_STATE.EXPLORATION;
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        HandleMovementInput();
        MoveCharacter();
    }

    void HandleMovementInput()
    {
        if (!isMoving)
        {
            input = InputManager.GetInstance().MovementInput();

            if (input.x != 0f && input.y != 0f)
            {
                input.x = Mathf.Sign(input.x);
                input.y = Mathf.Sign(input.y);
            }

            if (input != Vector2.zero)
            {
                Direction oldDirection = currentDir;
                Vector3Int moveDirection = Vector3Int.RoundToInt(new Vector3(input.x, input.y, 0f));

                if (moveDirection != Vector3Int.zero)
                {
                    currentDir = GetDirectionFromVector(moveDirection);
                }

                if (currentDir != oldDirection)
                {
                    remainingMoveDelay = moveDelay;
                }

                if (remainingMoveDelay > 0f)
                {
                    remainingMoveDelay -= 1f;
                    return;
                }

                Vector2 desiredVelocity = input * walkSpeed;
                moveVelocity = Vector2.Lerp(rb.velocity, desiredVelocity, 0.5f);
            }
            else
            {
                moveVelocity = Vector2.zero;
            }
        }
    }

    void MoveCharacter()
    {
        rb.velocity = moveVelocity;
    }

    private Direction GetDirectionFromVector(Vector3Int direction)
    {
        if (direction == Vector3Int.up)
            return Direction.North;
        else if (direction == Vector3Int.down)
            return Direction.South;
        else if (direction == Vector3Int.right)
            return Direction.East;
        else if (direction == Vector3Int.left)
            return Direction.West;
        else
            return currentDir;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") && InputManager.GetInstance().InteractInput())
        {
            collision.GetComponent<Boxes>().Activate(transform.position);
        }
    }
}

public enum Direction
{
    North, East, South, West
}
