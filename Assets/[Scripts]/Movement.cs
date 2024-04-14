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
    [SerializeField] bool canInteract = false;
    [SerializeField] private bool isInteracting = false;
    Vector3 startPos;
    Vector3 endPos;
    float progress;
    float remainingMoveDelay = 0f;
    float x;
    int framesPerMove = 60;
   [SerializeField] GameObject interactiveObject;
    GAME_STATE currentGamestate = default;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        DialogManager.GetInstance().OnCloseDialog += () =>
        {
            if (currentGamestate == GAME_STATE.READING)
            {
                interactiveObject.GetComponent<Istepable>().Deactivate();
                canInteract = true;
            }
        };
        
    }


    void FixedUpdate()
    {
        HandleMovementInput();
        MoveCharacter();
        if (currentGamestate == GAME_STATE.READING)
        {
            DialogManager.GetInstance().HandleUpdate();
        }
        SetInteraction();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Stepable")
        {
            interactiveObject = other.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Stepable")
        {
            interactiveObject = null;
            canInteract = false;
            isInteracting = false;
        }
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
    
    public void SetInteraction()
    {
        if (canInteract && InputManager.GetInstance().InteractInput())
        {
            if (interactiveObject != null)
            {
                interactiveObject.GetComponent<Istepable>().Activate();
                currentGamestate = GameManager.GetInstance().GetCurrentGameState();
                canInteract = false;
                isInteracting = true;
            }
        }
        
    }

   

}

public enum Direction
{
    North, East, South, West
}

