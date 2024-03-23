using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private float moveDelay = 0.2f;
    Direction currentDir = Direction.South;
    public Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float progress;
    float remainingMoveDelay = 0f;
    int framesPerMove = 60;
    GAME_STATE currentGamestate = default;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
            if (input.x != 0f && input.y != 0f)
            {
                input.x = Mathf.Sign(input.x);
                input.y = Mathf.Sign(input.y);
            }

            if (input != Vector2.zero)
            {
                Vector3Int moveDirection = Vector3Int.RoundToInt(new Vector3(input.x, input.y, 0f));

                if (moveDirection != Vector3Int.zero)
                {
                    currentDir = GetDirectionFromVector(moveDirection);
                    remainingMoveDelay = moveDelay;
                }

                if (remainingMoveDelay > 0f)
                {
                    remainingMoveDelay -= Time.deltaTime;
                    return;
                }

                startPos = transform.position;
                endPos = new Vector3(startPos.x + input.x, startPos.y + input.y, startPos.z);
                Vector3Int tilePosition = floorTilemap.WorldToCell(endPos);

                if (floorTilemap.GetTile(tilePosition) != null && wallTilemap.GetTile(tilePosition) == null)
                {
                    // Check for collisions with objects outside tilemaps
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(endPos, 0.1f);
                    bool canMove = true;
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.gameObject != gameObject) // Ignore self
                        {
                            canMove = false;
                            break;
                        }
                    }

                    if (canMove)
                    {
                        isMoving = true;
                        progress = 0f;
                    }

                }
            }
        }
    }

    void MoveCharacter()
    {
        if (isMoving)
        {
            if (progress < 1f)
            {
                progress += (1f / framesPerMove) * walkSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, progress);
            }
            else
            {
                isMoving = false;
                transform.position = endPos;
            }
        }
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
}

public enum Direction
{
    North, East, South, West
}