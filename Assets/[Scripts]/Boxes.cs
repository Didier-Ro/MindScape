using UnityEngine;
using UnityEngine.Tilemaps;

public class Boxes : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap = default;
    [SerializeField] private Tilemap wallTilemap = default;
    [SerializeField] private float boxDistance = 1f;
    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private float progress;
    private Vector2 colliderSize;
    private float lerpTime = 1f;
    private float currentLerpTime = 0f;

    private void FixedUpdate()
    {
        if (isMoving)
        {
            MoveBox();
        }
    }

    private void MoveBox()
    {
        currentLerpTime += Time.fixedDeltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
            isMoving = false;
        }

        float t = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, t);
    }

    public void Activate(Vector2 _playerDirection)
    {
        Vector2 distance = _playerDirection - (Vector2)transform.position;
        Vector2 dir = Vector2.zero;
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            startPos = transform.position;
            if (distance.x > 0)
            {
                dir = Vector2.left;
                endPos = new Vector3(startPos.x - boxDistance, startPos.y, startPos.z);
            }
            else
            {
                dir = Vector2.right;
                endPos = new Vector3(startPos.x + boxDistance, startPos.y, startPos.z);
            }
        }
        else
        {
            startPos = transform.position;
            if (distance.y > 0)
            {
                dir = Vector2.down;
                endPos = new Vector3(startPos.x, startPos.y - boxDistance, startPos.z);
            }
            else
            {
                dir = Vector2.up;
                endPos = new Vector3(startPos.x, startPos.y + boxDistance, startPos.z);
            }
        }

        // Check if the destination position is within the floor tilemap and not within the wall tilemap
        Vector3Int tilePosition = floorTilemap.WorldToCell(endPos);
        TileBase floorTile = floorTilemap.GetTile(tilePosition);
        TileBase wallTile = wallTilemap.GetTile(tilePosition);

        if (floorTile != null && wallTile == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, dir, boxDistance + 0.1f, LayerMask.GetMask("obstacleLayer"));
            if (hit.collider == null)
            {
                isMoving = true;
                currentLerpTime = 0f;
            }
            else
            {
                float distanceToCollision = Vector2.Distance(hit.point, transform.position);
                if (Mathf.Abs(distanceToCollision) > 0.1)
                {
                    Vector2 directionToMove = dir * (distanceToCollision);
                    transform.Translate(directionToMove);
                }
            }
        }
    }
}
