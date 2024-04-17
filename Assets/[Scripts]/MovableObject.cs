using System;
using UnityEngine;

public class MovableObject : MonoBehaviour, Istepable
{
    public direction pushDirection;
    public bool isMoving;
    public float timeToReachPointInSeconds = 1;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private GameObject activateObject;
    [SerializeField] private int distanceToMove;
    [SerializeField] private GameObject boxFalling;
    private int finalFramesToReachPoint = default; 
    private float speedPerFrame = default;
    private float finalDistanceToMove = default;
    private RaycastHit2D rayhit;
    private Vector2 directionToMove = Vector2.zero;
    private int frameCounter = 0;
    private Vector2 startPosition;
    private Vector2 finalPosition;
    private bool boxIsOnPrecipice;
    private ActivateZone activateScript;
    public void Activate()
    {
        speedPerFrame = finalDistanceToMove / (timeToReachPointInSeconds * 60);
        if (frameCounter <= finalFramesToReachPoint)
        {
            activateObject.transform.Translate(directionToMove * speedPerFrame);
            frameCounter++;
        }
        else
        {
            activateObject.transform.position = finalPosition;
            activateScript.ActivateBool();
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
        startPosition = activateObject.transform.position;
        if (rayhit.collider == null)
        {
            finalDistanceToMove = distanceToMove;
        }
        else
        {
            float approx = Mathf.Abs(rayhit.distance) + offset.magnitude;
            float difference = Mathf.Abs(distanceToMove + offset.magnitude - 0.01f - approx);
            if (difference <= 0.02f)
            {
                finalDistanceToMove = distanceToMove;
            }
            else
            {
                if (rayhit.collider.CompareTag("Precipice"))
                {
                    finalDistanceToMove = Mathf.Abs(Vector2.Distance(transform.position ,rayhit.collider.transform.position));
                    rayhit.collider.enabled = false;
                    boxIsOnPrecipice = true;
                }
                else
                {
                    Vector2 distance = rayhit.point - (Vector2)transform.position - offset;
                    finalDistanceToMove = distance.magnitude;
                    if (finalDistanceToMove < 0.1f)
                    {
                        activateScript.ActivateBool();
                        return;
                    }
                }
            }
             
        }
        finalFramesToReachPoint = (int)timeToReachPointInSeconds * 60;
        finalPosition = startPosition + finalDistanceToMove * directionToMove;
        frameCounter = 0;
        isMoving = true;
    }

    private void Start()
    {
        activateScript = activateObject.GetComponent<ActivateZone>();
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
        if (!isMoving && boxIsOnPrecipice)
        {
            Destroy(activateObject);
            if (boxFalling != null)
            {
                PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.FallingBox, transform.position, Vector2.zero);
            }
        }
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
