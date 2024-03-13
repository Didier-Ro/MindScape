using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boxes : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap = default;
    [SerializeField] private float moveDelay = default;
    [SerializeField] private float walkSpeed = default;
    [SerializeField] private float boxDistance = 1f;
    private bool isMoving = false;
    private bool isSuscribed = true;
    private Vector3 startPos;
    private Vector3 endPos;
    private float progress;
    private int framesPerMove = 60;
    private Vector2 colliderSize;



    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        isMoving = _newGameState == GAME_STATE.EXPLORATION;
    }

    #endregion

    private void Start()
    {
       colliderSize = GetComponent<Collider2D>().bounds.size;
    }

    public void Activate(Vector2 _playerDirection)
    {
        Vector2 distance = _playerDirection - (Vector2)transform.position;
        Vector2 dir = Vector2.zero;
        if (Mathf.Abs(distance.x) > MathF.Abs(distance.y))
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
       /* Vector3Int tilePosition = new Vector3Int((int)(endPos.x - 0.5f), (int)(endPos.y - 0.5f), 0);
        if (tilemap.GetTile(tilePosition) == null)
        {
            isMoving = true;
            progress = 0f;
        }*/
          RaycastHit2D hit = Physics2D.Raycast(startPos, dir, boxDistance, LayerMask.GetMask("obstacleLayer"));
          if(hit.collider==null)
          {
              transform.position = endPos;
          }
          else
          { 
              float distanceToCollision =  Vector2.Distance(hit.point,transform.position);
              float offset = colliderSize.x / 2;
              if (Mathf.Abs(distanceToCollision - offset) > 0.1)
              {
                  Vector2 directionToMove = dir * (distanceToCollision - offset);
                  transform.Translate(directionToMove);
              }
          }
    }
}