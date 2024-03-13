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
    public static InputAction interactInput = default;
    private InputAction pauseInput = default;
    private InputAction lightInput = default;
    private InputAction dashInput = default;

    [Header("ReadInputs")] 
    public static InputAction nextInput = default;

    [Header("Read values")] 
    private Vector2 vectorValue = default;
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
        vectorValue = moveInput.ReadValue<Vector2>();
        return vectorValue;
    }

    public bool FlashligthInput()
    {
        return lightInput.triggered;
    }
    public bool InteractInput()
    {
        return interactInput.triggered;
        //uwu
    }

    public bool NextInput()
    {
        return nextInput.triggered;
    }

    public bool DashInput()
    {
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
