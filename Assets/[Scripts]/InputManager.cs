using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    #region Singletone
    private static InputManager Instance;
    public static InputManager GetInstance() 
    { 
        return Instance;
    }
    #endregion
    #region SubscribeToGamestate

    public bool isCinematic = false;

    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        if (!isCinematic)
        {
            PlayerStates.GetInstance().OnPlayerStateChanged += PlayerStateChange;
            PlayerStateChange(PlayerStates.GetInstance().GetCurrentPlayerState());
        }
    }

    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and shows a different UI
    {
        switch (_newGameState)
        {
            case GAME_STATE.PAUSE:
                //PauseUI();
                break;
            case GAME_STATE.EXPLORATION:
                ActivateGameplay();
                break;
            case GAME_STATE.READING:
                ActivateReading();
                break;
            case GAME_STATE.DEAD:
                DeactivatePause();
                break;
            case GAME_STATE.FALLING:
                DeactivateInput();
                break;
            case GAME_STATE.LOADING:
                DeactivateInput();
                break;
            case GAME_STATE.TUTORIAL:
                playerControls.Gameplay.Pause.Disable();
                break;
           
        }   
    }
    
    private void PlayerStateChange(PLAYER_STATES _newPlayerState)//Analyze the Game State type and shows a different UI
    {
        switch (_newPlayerState)
        {
            case PLAYER_STATES.FALL:
                DeactivateInput();
                break;
            case PLAYER_STATES.PLAY:
                ActivateGameplay();
                break;
            case PLAYER_STATES.DASHING:
                DeactivateInput();
                break;
            case PLAYER_STATES.THROWBOX:
                DeactivateInput();
                break;
            case PLAYER_STATES.RESPAWN:
                DeactivatePause();
                break;
            case PLAYER_STATES.TUTORIAL:
                DeactivatePause();
                break;
            case PLAYER_STATES.PAUSE:
                playerControls.Gameplay.Dash.Disable();
                break;
        }   
    }
    
    #endregion

    public static PlayerInput playerInput = default;
    
    public static string gamepadControlScheme = "Gamepad";
    public static string keyboardControlScheme = "Keyboard";
    public static string currentControlScheme {get; private set;}
    
    [SerializeField] private InputActionReference actionReference;

    [HideInInspector] public PlayerControls playerControls = default;
    
    [Header("GameplayInputs")]
    private InputAction moveInput = default;
    private InputAction interactInput = default;
    private InputAction pauseInput = default;
    private InputAction lightInput = default;
    private InputAction dashInput = default;
    private InputAction moveLightInput = default;
    private InputAction focusInput = default;
    [HideInInspector] public InputAction tutorialInput = default;
    

    [Header("UIInputs")] 
    private InputAction nextUIInput = default;
    public static IObservable<InputControl> onAnyButtonPress { get; }

    private InputAction backUIInput = default;

    [Header("ReadInputs")] 
    private InputAction nextInput = default;

    [Header("Read values")] 
    private Vector2 vectorMovementValue = default;
    private Vector2 vectorLightValue = default;
    private bool isPaused = false;
    private bool isHolding = false;
    private bool anyButton = true;

    private void Awake()
    {
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            if (!isCinematic)
            {
                Destroy(gameObject);
            }
        }

        playerInput = GetComponent<PlayerInput>();
        playerControls = new PlayerControls();
        playerControls.Enable();
        moveInput = playerControls.Gameplay.Movement;
        moveInput.Enable();
        interactInput = playerControls.Gameplay.Interact;
        interactInput.Enable();
        pauseInput = playerControls.Gameplay.Pause;
        pauseInput.Enable();
        lightInput = playerControls.Gameplay.Protect;
        lightInput.Enable();
        dashInput = playerControls.Gameplay.Dash;
        dashInput.Enable();
        moveLightInput = playerControls.Gameplay.MoveLight;
        moveLightInput.Enable();
        nextInput = playerControls.Reading.Next;
        nextInput.Disable();
        nextUIInput = playerControls.UI.Next;
        nextUIInput.Enable();
        backUIInput = playerControls.UI.Back;
        backUIInput.Enable();
        focusInput = playerControls.Gameplay.FocusNext;
        focusInput.Enable();
        playerControls.Gameplay.Pause.performed += _ => SetPause();
        actionReference.action.Enable();

        

    }
    
    private void Start()
    {
        SubscribeToGameManagerGameState();
        SetHolding();
            
    }

    private void FixedUpdate()
    {
        if (GameManager.GetInstance().GetCurrentGameState() == GAME_STATE.DEAD)
        {
            InputSystem.onAnyButtonPress.CallOnce(ctrl => anyButton = ctrl.IsPressed());
        }
        if (!anyButton)
        {
            LoadingManager.instance.LoadScene("MainMenu");
            anyButton = true;
        }
    }

    private void OnDisable()
    {
        playerControls.Disable();
        actionReference.action.Disable();
    }
    
    
    public bool SetPause()
    {
        if (GameManager.GetInstance().GetCurrentGameState() != GAME_STATE.EXPLORATION || GameManager.GetInstance().GetCurrentGameState() != GAME_STATE.PAUSE)
        {
            return pauseInput.triggered;
        }

        return false;
    }
    
    public Vector2 MovementInput()
    {
        vectorMovementValue = moveInput.ReadValue<Vector2>();
        return vectorMovementValue;
    }

    public Vector2 MoveLightInput()
    {
        vectorLightValue = moveLightInput.ReadValue<Vector2>();
        return vectorLightValue;
    }

    public bool FlashligthInput()
    {
        return lightInput.triggered;
    }
    public bool InteractInput()
    {
        return interactInput.triggered;
    }
    

    public bool NextInput()
    {
        return nextInput.triggered;
    }

    public bool NextUIInput()
    {
        return nextUIInput.triggered;
    }

    public bool BackUIInput()
    {
        return backUIInput.triggered;
    }

    public bool DashInput()
    {
        return dashInput.triggered;
    }

    public bool HoldingInteract()
    {
        return isHolding;
    }

    public bool FocusNextGoal()
    {
        return focusInput.triggered;
    }

    public void ReturnAnyButton()
    {
        
    }
    
    public void SwitchControls(PlayerInput input)
    {
        currentControlScheme = input.currentControlScheme;
        Debug.Log(currentControlScheme);
    }

    public string ReturnControlScheme(string _currentControlScheme)
    {
        _currentControlScheme = currentControlScheme;
        return _currentControlScheme;
    }

    private void SetHolding()
    {
        actionReference.action.started += context =>
        {
            if (context.interaction is HoldInteraction)
            {
               // Debug.Log(context.interaction + "Started");
                isHolding = true;
            }else if (context.interaction is TapInteraction)
            {
               // Debug.Log(context.interaction + "Started");
                isHolding = false;
            }
        };

        actionReference.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                //Debug.Log(context.interaction + "Performed");
                isHolding = true;
            }else if (context.interaction is TapInteraction)
            {
                //Debug.Log(context.interaction + "performed");
                isHolding = false;
            }
        };
        
        actionReference.action.canceled += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                //Debug.Log(context.interaction + "canceled");
                isHolding = false;
            }else if (context.interaction is TapInteraction)
            {
                //Debug.Log(context.interaction + "canceled");
                isHolding = false;
            }
        };


    }

    private void ActivateReading()
    {
      playerControls.Gameplay.Disable();
       playerControls.Reading.Enable();
    }

    private void ActivateGameplay()
    {
        playerControls.Gameplay.Enable();
        playerControls.Reading.Disable();
    }

    private void DeactivateInput()
    {
        playerControls.Gameplay.Disable();
        playerControls.Reading.Disable();
    }

    private void DeactivatePause()
    {
        playerControls.Gameplay.Disable();
    }
    
}

public enum ACTIVECONTROLSCHEME
{
    CONTROL,
    KEYBOARD
}