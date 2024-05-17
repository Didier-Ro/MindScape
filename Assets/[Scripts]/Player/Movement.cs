using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2 input;
    public bool isMoving = true;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] bool canInteract = false;
    [SerializeField] private bool isInteracting = false;
    [SerializeField] GameObject interactiveObject;
    private bool isSuscribed = true;
    private float timeSinceLastStep = 0f; // Tiempo transcurrido desde el último paso
    private float stepDelay = 0.5f; // Retraso entre pasos en segundos

    #region CenterPlayerToABox
    private bool isThePlayerCenterToTheBox;   
    private bool isMovingToCenterOfTheBox = false;
    private Vector2 positionToCenterThePlayer;
    #endregion
    private bool canMove = true;
    public ActivateZone _activateZone;
    private GAME_STATE currentGamestate = default;
    [SerializeField] Animator animator;
    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D Rb { get { return rb; } }

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        Debug.Log(_newGameState.ToString());
       isMoving = _newGameState == GAME_STATE.EXPLORATION;
    }

    #endregion


    #region SubscriptionToPlayerStates
    
    private void SubscribeToPlayerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        if (PlayerStates.GetInstance() != null)
        {
            PlayerStates.GetInstance().OnPlayerStateChanged += OnPlayerStateChange;
            OnPlayerStateChange(PlayerStates.GetInstance().GetCurrentPlayerState());
        }
    }
    
    private void OnPlayerStateChange(PLAYER_STATES _newPlayerState)
    {
        if (_newPlayerState == PLAYER_STATES.PLAY)
        {
            canMove = true;
            isThePlayerCenterToTheBox = false;
        }
        else
        {
            canMove = false;
        }
    }

    

    #endregion
    private void Start()
    {
        SubscribeToGameManagerGameState();
        SubscribeToPlayerGameState();
        rb = GetComponent<Rigidbody2D>();
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
        input = InputManager.GetInstance().MovementInput();
        HandleMovementInput();
        if (currentGamestate == GAME_STATE.READING)
        {
            DialogManager.GetInstance().HandleUpdate();
            isMoving = false;
        }
        if (InputManager.GetInstance().HoldingInteract() && _activateZone != null && _activateZone.canActivate)
        {
            interactiveObject.GetComponent<ActivateZone>().ActivateBoxProcess();
        }
        if (isMovingToCenterOfTheBox)
        {
            MoveThePlayerToABox();
            
        }
        
        
    }
    
    public void CenterThePlayerToABox(Vector2 positionToMove)
    {
        positionToCenterThePlayer = positionToMove;
        isMovingToCenterOfTheBox = true;
    }

    public bool IsTheBoxCenter()
    {
        return isThePlayerCenterToTheBox;
    }

    private void MoveThePlayerToABox()
    {
        if (Vector2.Distance(positionToCenterThePlayer, transform.position) < 0.05f)
        {
            isMovingToCenterOfTheBox = false;
            isThePlayerCenterToTheBox = true;
            transform.position = positionToCenterThePlayer;
            positionToCenterThePlayer = Vector2.zero;
        }
        else
        { 
            Vector2 newPos = Vector2.MoveTowards(transform.position, positionToCenterThePlayer, 0.01f); transform.position = (newPos);
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stepable"))
        {
            interactiveObject = other.gameObject;
            _activateZone = interactiveObject.GetComponent<ActivateZone>();
            canInteract = true;
        }
        if (other.GetComponent<ActivateZone>())
        {
            interactiveObject = other.gameObject;
            _activateZone = interactiveObject.GetComponent<ActivateZone>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stepable"))
        {
            interactiveObject = null;
            canInteract = false;
            isInteracting = false;
        }
       
    }

    void HandleMovementInput()
    {
        if (isMoving)
        {
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
            animator.SetFloat("Speed", input.magnitude);
            Vector2 movement = input.normalized * walkSpeed * Time.fixedDeltaTime;
            if (canMove)
            {
                rb.MovePosition(rb.position + movement);
                timeSinceLastStep += Time.fixedDeltaTime;
                if (timeSinceLastStep > stepDelay)
                {
                    GameObject particle = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Pasos, rb.position, Vector3.zero);
                    ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        particleSystem.Play();
                    }
                    timeSinceLastStep = 0f;
                }
            }
        }
    }
}

