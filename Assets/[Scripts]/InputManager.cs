using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private PlayerControls playerControls = default;
    
    [Header("GameplayInputs")]
    private InputAction moveInput = default;
    private InputAction interactInput = default;
    private InputAction pauseInput = default;
    private InputAction lightInput = default;
    private InputAction dashInput = default;
    private InputAction moveLightInput = default;

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
        playerControls.Gameplay.Pause.performed += _ => SetPause();

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

    public bool DashInput()
    {
        Debug.Log("funciona el input");
        return dashInput.triggered;
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
