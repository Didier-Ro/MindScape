using System;
using UnityEngine;

public class MovableObject : MonoBehaviour, Istepable
{
    public direction pushDirection;
    public bool isMoving;
    private int finalFramesToReachPoint = default; 
    private float speedPerFrame = default;
    private float finalDistanceToMove = default;
    private RaycastHit2D rayhit;
    private Vector2 directionToMove = Vector2.zero;
    private int frameCounter = 0;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    private Vector2 startPosition;
    private Vector2 finalPosition;
    [SerializeField] private int distanceToMove;
    public float timeToReachPointInSeconds = 1;
    public void Activate()
    {
        speedPerFrame = finalDistanceToMove / (timeToReachPointInSeconds * 60);
        if (frameCounter <= finalFramesToReachPoint)
        {
            transform.Translate(directionToMove * speedPerFrame);
            frameCounter++;
        }
        else
        {
            transform.position = finalPosition;
            isMoving = false;
        }
    }
    private void RayCastCheck()
    {
        Vector2 offset = Vector2.zero;
        switch (pushDirection)
        { 
            case direction.right:
                offset = new Vector2(-offsetX - 0.01f, 0);
                directionToMove = Vector2.left;
                break;
            case direction.left:
                offset = new Vector2(offsetX + 0.01f, 0);
                directionToMove = Vector2.right;
                break;
            case direction.up:
                offset = new Vector2(0, -offsetY - 0.01f);
                directionToMove = Vector2.down;
                break;
            case direction.down:
                offset = new Vector2(0, offsetY + 0.01f);
                directionToMove = Vector2.up;
                break;
        }
        Debug.DrawRay((Vector2) transform.position + offset, directionToMove * distanceToMove, Color.red, 1f);
        rayhit = Physics2D.Raycast((Vector2)transform.position + offset, directionToMove, distanceToMove);
        startPosition = transform.position;
        if (rayhit.collider == null)
        {
            finalDistanceToMove = distanceToMove;
        }
        else
        {
            Vector2 distance = rayhit.point - (Vector2)transform.position - offset;
            finalDistanceToMove = distance.magnitude;
        }
        finalFramesToReachPoint = (int)timeToReachPointInSeconds * 60;
        finalPosition = startPosition + finalDistanceToMove * directionToMove;
        frameCounter = 0;
        isMoving = true;
    }
    public void GetDirection(Vector2 _playerDirection)
    {
        if (!isMoving)
        {
            Vector2 distance = _playerDirection - (Vector2)transform.position;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                pushDirection = distance.x > 0 ? direction.right : direction.left;
            }
            else
            {
                pushDirection = distance.y > 0 ? direction.up : direction.down;
            }
            RayCastCheck();  
        }
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        Activate();
    }

    public void Deactivate()
    {
        
    }
}
public enum direction
{
    up, down, left, right,
}
