using UnityEngine;
using UnityEngine.Tilemaps;
public class Movement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public Tilemap tilemap;
    public float moveDelay = 0.2f;
    Direction currentDir = Direction.South;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos;
    Vector3 endPos;
    float progress;
    float remainingMoveDelay = 0f;

    public void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x != 0f)
                input.y = 0;
            if (input != Vector2.zero)
            {
                Direction oldDirection = currentDir;
                if (input.x == -1f)
                    currentDir = Direction.West;
                if (input.x == 1f)
                    currentDir = Direction.East;
                if (input.y == 1f)
                    currentDir = Direction.North;
                if (input.y == -1f)
                    currentDir = Direction.South;

                if (currentDir != oldDirection)
                {
                    remainingMoveDelay = moveDelay;
                }
                if (remainingMoveDelay > 0f)
                {
                    remainingMoveDelay -= Time.deltaTime;
                    return;
                }
                startPos = transform.position;
                endPos = new Vector3(startPos.x + input.x, startPos.y + input.y, startPos.z);
                Vector3Int tilePosition = new Vector3Int((int)(endPos.x - 0.5f),
                                                         (int)(endPos.y - 0.5f), 0);

                if (tilemap.GetTile(tilePosition) == null)
                {
                    isMoving = true;
                    progress = 0f;
                }
            }
        }
        if (isMoving)
        {
            if (progress < 1f)
            {
                progress += Time.deltaTime * walkSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, progress);
            }
            else
            {

                isMoving = false;
                transform.position = endPos;
            }
        }
    }
}
enum Direction
{
    North, East, South, West
}