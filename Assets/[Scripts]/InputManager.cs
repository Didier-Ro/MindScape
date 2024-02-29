using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{


    private PlayerControls _playerControls = default;
    private InputAction _moveInput = default;
    private InputAction _interactInput = default;
    //Here goes any script that you want to control
    private ONLYTESTmovement _playerMovement = default;

    [Header("Read values")] 
    private Vector2 _vectorValue = default;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerMovement = GetComponent<ONLYTESTmovement>();
        _playerControls.Enable();
        _moveInput = _playerControls.Gameplay.Movement;
        _moveInput.Enable();
        _interactInput = _playerControls.Gameplay.Interact;
        _interactInput.Enable();
    }
    

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 MovementInput()
    {
        _vectorValue = _moveInput.ReadValue<Vector2>();
        Debug.Log(_vectorValue);
        return _vectorValue;
    }
}
