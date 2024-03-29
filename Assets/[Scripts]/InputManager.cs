using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    private void Start()
    {
        SubscribeToGameManagerGameState();
    }

    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
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
                //DeadUI();
                break;
        }   
    }
    #endregion

    public static PlayerInput playerInput = default;
    public static string gamepadControlScheme = "Gamepad";
    public static string keyboardControlScheme = "Keyboard";
    public static string currentControlScheme {get; private set;}

    private PlayerControls playerControls = default;
    
    [Header("GameplayInputs")]
    private InputAction moveInput = default;
    private InputAction interactInput = default;
    private InputAction pauseInput = default;
    private InputAction lightInput = default;
    private InputAction dashInput = default;
    private InputAction moveLightInput = default;

    [Header("UIInputs")] 
    private InputAction nextUIInput = default;

    private InputAction backUIInput = default;

    [Header("ReadInputs")] 
    private InputAction nextInput = default;

    [Header("Read values")] 
    private Vector2 vectorMovementValue = default;
    private Vector2 vectorLightValue = default;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
        playerControls.Gameplay.Pause.performed += _ => SetPause();

    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
    
    public bool SetPause()
    {
        return pauseInput.triggered;
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
        Debug.Log("hola");
        return dashInput.triggered;
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
    
}
