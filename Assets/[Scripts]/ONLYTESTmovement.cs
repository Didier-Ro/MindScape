using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class ONLYTESTmovement : MonoBehaviour
{
    [SerializeField] private int _speed = default;
    private Vector2 _moveInputValue = Vector2.zero;
    private InputManager _inputManager = default;
    private Rigidbody2D _rb = default;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _rb = GetComponent<Rigidbody2D>();
    }


    public void Move()
    {
        _moveInputValue = _inputManager.MovementInput();
        _rb.velocity = new Vector2(_moveInputValue.x * _speed, _moveInputValue.y * _speed);
    }
    
    private void FixedUpdate()
    {
        Move();
    }
}
