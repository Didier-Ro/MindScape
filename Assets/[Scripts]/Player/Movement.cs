using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    public Vector2 input;
    private bool canWatchTarget = true;
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
    [SerializeField] private SoundLibrary soundLibrary;

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
       if (!isMoving)
       {
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
            case PLAYER_STATES.TARGET_CAMERA:
                canMove = false;
                canWatchTarget = true;
                break;
            case PLAYER_STATES.MOVING_CAMERA:
                canMove = false;
                canWatchTarget = true;
                break;
            case PLAYER_STATES.RESPAWN:
                canMove= false;
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
        if (canWatchTarget && InputManager.GetInstance().FocusNextGoal())
        {
            FocusNextTarget();
        }
        if (PlayerStates.GetInstance().GetCurrentPlayerState() != PLAYER_STATES.MOVING_CAMERA)
            return;
        if (CameraManager.instance.HasCameraArrive(PlayerStates.GetInstance().gameObject))
        {
            //PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        }
        else if(CameraManager.instance.HasCameraArrive(CameraManager.instance.targetPuzzle))
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.TARGET_CAMERA);
        }
    }

    private void FocusNextTarget()
    {
        if (PlayerStates.GetInstance().GetCurrentPlayerState() == PLAYER_STATES.PLAY)
        {
            if (CameraManager.instance.targetPuzzle != null)
            {
                CameraManager.instance.ChangeCameraToAnObject(CameraManager.instance.targetPuzzle);
            }
            
        }
        else if(PlayerStates.GetInstance().GetCurrentPlayerState() == PLAYER_STATES.TARGET_CAMERA)
        {
            CameraManager.instance.ChangeCameraToThePlayer();
        }
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.MOVING_CAMERA);
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

    private bool isPlayingSound = false; // Variable para controlar si se está reproduciendo un sonido

    void HandleMovementInput()
    {
        if (isMoving && canMove)
        {
            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
            animator.SetFloat("Speed", input.magnitude);
            Vector2 movement = input.normalized * walkSpeed * Time.fixedDeltaTime;
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

                // Reproducir el sonido LAMP_JIGGLE si no se está reproduciendo actualmente
                if (!isPlayingSound && soundLibrary != null)
                {
                    AudioClip jiggleSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.LAMP_JIGGLE);
                    if (jiggleSound != null)
                    {
                        isPlayingSound = true; // Marcar que se está reproduciendo un sonido
                        AudioSource.PlayClipAtPoint(jiggleSound, transform.position, 1f);
                        StartCoroutine(ResetSoundFlag(jiggleSound.length)); // Restablecer la bandera después de que termine el sonido
                    }
                }
            }
        }
    }

    // Corrutina para restablecer la bandera después de que termine el sonido
    private IEnumerator ResetSoundFlag(float soundLength)
    {
        yield return new WaitForSeconds(soundLength);
        isPlayingSound = false; // Restablecer la bandera
    }
}

