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

    public void Activate(Vector2 _playerDirection)
    {
        Vector2 distance = _playerDirection - (Vector2)transform.position;
        Vector2 dir = Vector2.zero;
        if (Mathf.Abs(distance.x) > MathF.Abs(distance.y))
        {
            startPos = transform.position;
            if (distance.x > 0)
            {
                dir = Vector2.right;
                endPos = new Vector3(startPos.x - boxDistance, startPos.y, startPos.z);
            }
            else
            {
                dir = Vector2.left;
                endPos = new Vector3(startPos.x + boxDistance, startPos.y, startPos.z);
            }

        }
        else
        {
            startPos = transform.position;
            if (distance.y > 0)
            {
                dir = Vector2.up;
                endPos = new Vector3(startPos.x, startPos.y - boxDistance, startPos.z);
            }
            else
            {
                dir = Vector2.down;
                endPos = new Vector3(startPos.x, startPos.y + boxDistance, startPos.z);
            }
        }
        Debug.Log(endPos);
        Vector3Int tilePosition = new Vector3Int((int)(endPos.x - 0.5f), (int)(endPos.y - 0.5f), 0);
        if (tilemap.GetTile(tilePosition) == null)
        {
            isMoving = true;
            progress = 0f;
        }
        transform.position = endPos;
        //RaycastHit2D hit = Physics2D.Raycast(startPos, dir, boxDistance, LayerMask.GetMask("obstacleLayer"));
        /*if(hit==null)
        {
            Debug.Log("No golpeo");
            
        }
        else
        {
            Debug.Log("Golpeo");
        }*/


    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}