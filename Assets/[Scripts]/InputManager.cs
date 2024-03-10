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

    private PlayerControls _playerControls = default;
    [Header("GameplayInputs")]
    private InputAction _moveInput = default;
    public static InputAction _interactInput = default;
    private InputAction _pauseInput = default;
    private InputAction _lightInput = default;
    private InputAction _dashInput = default;

    [Header("ReadInputs")] public static InputAction _nextInput = default;

    [Header("Read values")] 
    private Vector2 _vectorValue = default;
    private bool _isPaused = false;

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
        
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        _moveInput = _playerControls.Gameplay.Movement;
        _moveInput.Enable();
        _interactInput = _playerControls.Gameplay.Interact;
        _interactInput.Enable();
        _pauseInput = _playerControls.Gameplay.Pause;
        _pauseInput.Enable();
        _lightInput = _playerControls.Gameplay.Protect;
        _lightInput.Enable();
        _dashInput = _playerControls.Gameplay.Dash;
        _dashInput.Enable();
        _nextInput = _playerControls.Reading.Next;
        _nextInput.Disable();
        _playerControls.Gameplay.Pause.performed += _ => SetPause();

    }

    private void ActivateReading()
    {
      _playerControls.Gameplay.Disable();
       _playerControls.Reading.Enable();
    }

    private void ActivateGameplay()
    {
        _playerControls.Gameplay.Enable();
        _playerControls.Reading.Disable();
    }


    private void OnDisable()
    {
        _playerControls.Disable();
    }
    
    public Vector2 MovementInput()
    {
        _vectorValue = _moveInput.ReadValue<Vector2>();
        return _vectorValue;
    }

    public bool SetPause()
    {
        return _pauseInput.triggered;
    }

    public bool InteractInput()
    {
        return _interactInput.triggered;
    }

    public bool FlashligthInput()
    {
        return _lightInput.triggered;
    }

    public bool DashInput()
    {
        return _dashInput.triggered;
    }

    public bool NextInput()
    {
        return _nextInput.triggered;
    }
}
