using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    public Vector2 input;
    private bool canWatchTarget = true;
    public bool isMoving = true;
    [SerializeField] private float walkSpeed = 1.5f;
    private float actualSpeed;
    [SerializeField] bool canInteract = false;
    [SerializeField] private bool isInteracting = false;
    [SerializeField] GameObject interactiveObject;
    private bool isSuscribed = true;
    private float timeSinceLastStep = 0f;
    private float stepDelay = 0.5f;

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
    [SerializeField] private SoundLibrary soundLibrary;

    // Nueva referencia al punto de los pies
    [SerializeField] private Transform feetPosition;

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        if (_newGameState == GAME_STATE.EXPLORATION)
        {
            isMoving = true;
            actualSpeed = walkSpeed;
        }
        else if (_newGameState == GAME_STATE.TUTORIAL)
        {
            isMoving = true;
            actualSpeed /= 10f;
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
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
        switch (_newPlayerState)
        {
            case PLAYER_STATES.PLAY:
                canMove = true;
                canWatchTarget = true;
                break;
            case PLAYER_STATES.MOVING_CAMERA:
                canMove = false;
                canWatchTarget = false;
                Invoke("ChangeToPlayer", 3f);
                break;
            case PLAYER_STATES.TUTORIAL:
                canMove = false;
                canWatchTarget = true;
                break;
            default:
                canMove = false;
                canWatchTarget = false;
                break;
        }
    }



    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SubscribeToGameManagerGameState();
        SubscribeToPlayerGameState();
        DialogManager.GetInstance().OnCloseDialog += () =>
        {
            if (currentGamestate == GAME_STATE.READING)
            {
                interactiveObject.GetComponent<Istepable>().Deactivate();
                canInteract = true;
            }
        };
        actualSpeed = walkSpeed;
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
        if (canWatchTarget && InputManager.GetInstance().FocusNextGoal())
        {
            FocusNextTarget();
        }
    }

    private void FocusNextTarget()
    {
        if (CameraManager.instance.targetPuzzle != null)
        {
            CameraManager.instance.ChangeCameraToAnObject(CameraManager.instance.targetPuzzle);
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.MOVING_CAMERA);
        }
    }

    private void ChangeToPlayer()
    {
        CameraManager.instance.ChangeCameraToThePlayer();
        Invoke("CanWatchTarget", 2f);
    }

    private void CanWatchTarget()
    {
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
    }

    /*public void CenterThePlayerToABox(Vector2 positionToMove)
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
    }*/

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

    private bool isPlayingSound = false;

    void HandleMovementInput()
    {
        if (isMoving && canMove)
        {
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
            animator.SetFloat("Speed", input.magnitude);
            Vector2 movement = input.normalized * actualSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            // Verificar si el personaje est� en movimiento
            if (input.magnitude > 0)
            {
                timeSinceLastStep += Time.fixedDeltaTime;
                if (timeSinceLastStep > stepDelay)
                {
                    GameObject particle = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Pasos, feetPosition.position, Vector3.zero);
                    ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        particleSystem.Play();
                    }
                    timeSinceLastStep = 0f;

                    if (!isPlayingSound && soundLibrary != null)
                    {
                        AudioClip jiggleSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.LAMP_JIGGLE);
                        if (jiggleSound != null)
                        {
                            isPlayingSound = true;
                            AudioSource.PlayClipAtPoint(jiggleSound, transform.position, 5f);
                            StartCoroutine(ResetSoundFlag(jiggleSound.length));
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ResetSoundFlag(float soundLength)
    {
        yield return new WaitForSeconds(soundLength);
        isPlayingSound = false;
    }
}