using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private float moveDelay = 0.2f;
    Direction currentDir = Direction.South;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float progress;
    float remainingMoveDelay = 0f;
    int framesPerMove = 60;

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

                startPos = transform.position;
                endPos = new Vector3(startPos.x + input.x, startPos.y + input.y, startPos.z);
                Vector3Int tilePosition = floorTilemap.WorldToCell(endPos);

                if (floorTilemap.GetTile(tilePosition) != null && wallTilemap.GetTile(tilePosition) == null)
                {
                    isMoving = true;
                    progress = 0f;
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

