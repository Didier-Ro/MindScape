using System;
using System.Collections;
using System.Collections.Generic;
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

    private PlayerControls _playerControls = default;
    private InputAction _moveInput = default;
    private InputAction _interactInput = default;
    private InputAction _pauseInput = default;
    //Here goes any script that you want to control
    private ONLYTESTmovement _playerMovement = default;

    [Header("Read values")] 
    private Vector2 _vectorValue = default;
    private bool _isPaused = false;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerMovement = GetComponent<ONLYTESTmovement>();
        _playerControls.Enable();
        _moveInput = _playerControls.Gameplay.Movement;
        _moveInput.Enable();
        _interactInput = _playerControls.Gameplay.Interact;
        _interactInput.Enable();
        _pauseInput = _playerControls.Gameplay.Pause;
        _pauseInput.Enable();
        _playerControls.Gameplay.Pause.performed += _ => SetPause();
    }
    

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 MovementInput()
    {
       GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        _vectorValue = _moveInput.ReadValue<Vector2>();
        Debug.Log(_vectorValue);
        return _vectorValue;
    }

    public bool SetPause()
    {
        GameManager.GetInstance().ChangeGameState(GAME_STATE.PAUSE);
        _isPaused = true;
        return _isPaused;
    }
}
