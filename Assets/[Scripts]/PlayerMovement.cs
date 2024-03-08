using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _moveInputValue = Vector2.zero;
    public Animator animator;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
       
        MovePlayer();
    }
    
    void MovePlayer()
    {
        _moveInputValue = InputManager.GetInstance().MovementInput();
       
        Vector2 movement = new Vector2(_moveInputValue.x, _moveInputValue.y);
        animator.SetFloat("x", _moveInputValue.x);

        movement.Normalize(); // Evitar movimientos diagonales más rápidos

        _rb.velocity = movement * _speed;

        // Rotar el jugador hacia la dirección del movimiento
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    
}
