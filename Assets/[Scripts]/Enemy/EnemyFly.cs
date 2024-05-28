using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : MonoBehaviour, Ikillable
{
    private bool isSuscribed = true;
    private bool CanMove = true;
    private float amplitude = 1f;
    [SerializeField] private float chasingSpeed = default;
    private Rigidbody2D rb = default;
    private float sineCounter;
    public float frequency = 1f; 
    private Animator animator;

    #region SubscriptionToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        isSuscribed = true;
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and makes differents behaviour
    {
        CanMove = _newGameState == GAME_STATE.EXPLORATION;
        if (!CanMove)
        {
            rb.velocity = Vector2.zero;
        }
    }
    #endregion


    private void Start()
    {
        SubscribeToGameManagerGameState();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        ChasingProcess();
        
    }

    private void ChasingProcess()
    {
        Vector2 direction = PlayerStates.GetInstance().transform.position - transform.position;
        direction.Normalize();
        Vector2 perpendicularDirection = new Vector2(-direction.y, direction.x);
        float sineOffset = Mathf.Sin(sineCounter * frequency) * amplitude;
        Vector2 velocity = (direction * chasingSpeed) + (perpendicularDirection * sineOffset);
        rb.velocity = velocity;
        sineCounter += 0.1f;
    }


    private void OnDisable()
    {
        if (isSuscribed)
        {
            GameManager.GetInstance().OnGameStateChange -= OnGameStateChange;
            isSuscribed = false;
        }
    }
    
    private void OnEnable()
    {
        if (!isSuscribed)
        {
            GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
            OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
            isSuscribed = true;
        }
    }


    public void Hit(Transform player)
    {
        gameObject.SetActive(false);
    }

    public void UnHit(Transform player)
    {
    }
}
