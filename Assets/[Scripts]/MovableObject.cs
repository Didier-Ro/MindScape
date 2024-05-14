using UnityEngine;

public class MovableObject : MonoBehaviour, Istepable
{
    public direction pushDirection;
    public bool isMoving;
    public float timeToReachPointInSeconds = 1;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform[] vectorsToCenterThePlayer;
    [SerializeField] private GameObject activateObject;
    [SerializeField] private int distanceToMove;
    [SerializeField] private GameObject boxFalling;
    [SerializeField] private Animator animator;
    private int finalFramesToReachPoint = default;
    private Transform targetTransfom;
    private float speedPerFrame = default;
    private float finalDistanceToMove = default;
    private RaycastHit2D rayhit;
    private Vector2 directionToMove = Vector2.zero;
    private int frameCounter = 0;
    private Movement playerMovement;
    private Vector2 startPosition;
    private Vector2 finalPosition;
    private bool boxIsOnPrecipice;

    private void Start()
    {
        animator = PlayerStates.GetInstance().buttonsAnimator;
    }

    public void Activate()
    {
        speedPerFrame = finalDistanceToMove / (timeToReachPointInSeconds * 60);
        if (frameCounter <= finalFramesToReachPoint)
        {
            activateObject.transform.Translate(directionToMove * speedPerFrame);
            targetTransfom.Translate(directionToMove * speedPerFrame);
            frameCounter++;
        }
        else
        {
            activateObject.transform.position = finalPosition;
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
            isMoving = false;
        }
    }

    private Vector2 AddOffsetToRayCast(bool isPushing)
    {
        Vector2 offset = Vector2.zero;
        switch (pushDirection)
        { 
            case direction.right:
                offset = isPushing ? new Vector2(-offsetX - 0.01f, 0) : new Vector2(offsetX + 0.01f + 1f, 0);
                directionToMove = isPushing ? Vector2.left : Vector2.right;
                break;
            case direction.left:
                offset = isPushing ? new Vector2(offsetX + 0.01f, 0) : new Vector2(-offsetX - 0.01f - 1f, 0);
                directionToMove = isPushing ? Vector2.right : Vector2.left;
                break;
            case direction.up:
                offset = isPushing ? new Vector2(0, -offsetY - 0.01f) : new Vector2(0, offsetY + 0.01f +1f);
                directionToMove = isPushing ? Vector2.down : Vector2.up;
                break;
            case direction.down:
                offset = isPushing ? new Vector2(0,  offsetY + 0.01f) : new Vector2(0, -offsetY - 0.01f - 1f);
                directionToMove = isPushing ? Vector2.up : Vector2.down;
                break;
        }
        return offset;
    }
    
    private void RayCastCheck(bool isPushing)
    {
        Vector2 offset = AddOffsetToRayCast(isPushing);
        Debug.Log(offset);
        Debug.DrawRay((Vector2) transform.position + offset, directionToMove * distanceToMove, Color.red, 1f);
        rayhit = Physics2D.Raycast((Vector2)transform.position + offset, directionToMove, distanceToMove, _layerMask);
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
                if (rayhit.collider.GetComponent<IBoxInteraction>() != null)
                {
                    boxFalling = rayhit.collider.gameObject;    
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

    private float GetDirectionToMove()
    {
        float inputValue = 0;
        switch (pushDirection)
        {
            case direction.up:
                inputValue = -InputManager.GetInstance().MovementInput().y;
                break;
            case direction.down:
                inputValue = InputManager.GetInstance().MovementInput().y;
                break;
            case direction.left:
                inputValue = InputManager.GetInstance().MovementInput().x;
                break;
            case direction.right:
                inputValue = -InputManager.GetInstance().MovementInput().x;
                break;
        }
        return inputValue;
    }

    public void GetDirection(GameObject _target)
    {
        if (!isMoving)
        { 
            targetTransfom = _target.transform; 
            playerMovement = _target.GetComponent<Movement>();
            Vector2 distance = (Vector2)targetTransfom.position - (Vector2)transform.position;
            Vector2 vectorToCenter = Vector2.zero;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
            {
                pushDirection = distance.x > 0 ? direction.right : direction.left; 
                animator.SetBool("IsHoldingRL", true);
                animator.SetBool("IsHoldingUD", false);
                vectorToCenter = distance.x > 0 ? vectorsToCenterThePlayer[0].position : vectorsToCenterThePlayer[1].position;
            }
            else
            {
                pushDirection = distance.y > 0 ? direction.up : direction.down;
                animator.SetBool("IsHoldingRL", false);
                animator.SetBool("IsHoldingUD", true);
                vectorToCenter = distance.y > 0 ? vectorsToCenterThePlayer[2].position : vectorsToCenterThePlayer[3].position;
            }
            if (!playerMovement.IsTheBoxCenter())
            {
                playerMovement.CenterThePlayerToABox(vectorToCenter);
            }
            else
            {
                if (Mathf.Abs(GetDirectionToMove()) > 0)
                {
                    RayCastCheck(GetDirectionToMove() > 0);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isMoving && boxIsOnPrecipice)
        {
            if (boxFalling != null)
            {
                boxIsOnPrecipice = false;
                activateObject.GetComponent<ActivateZone>().canActivate = false;
                boxFalling.GetComponent<IBoxInteraction>().Activate(activateObject);
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
