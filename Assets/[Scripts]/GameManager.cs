using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager Instance;
    public static GameManager GetInstance() 
    { 
        return Instance;
    }
    #endregion

    private bool isPaused;
    private bool isFlashing;
    private GAME_STATE currentGameState = GAME_STATE.EXPLORATION;
    public Action<GAME_STATE> OnGameStateChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (InputManager.GetInstance().FlashligthInput())
        {
           ToggleFlash();
        }

        if (InputManager.GetInstance().SetPause())
        {
            GAME_STATE actualGameState = TogglePause() ? GAME_STATE.PAUSE : GAME_STATE.EXPLORATION;
            ChangeGameState(actualGameState);
        }
    }

    public bool GetFlashing()
    {
        return isFlashing;
    }

    private bool TogglePause()
    {
        isPaused = !isPaused;
        return isPaused;
    }

    public void ToggleFlash()
    {
        isFlashing = !isFlashing;
    }

    public void ChangeGameState(GAME_STATE _newGameState)//When called, the current Game State changes to the new Game State and sends a notification to all subscribers that the Game State changed
    {
        currentGameState = _newGameState;

        if (OnGameStateChange != null) 
        {
            OnGameStateChange.Invoke(currentGameState);
        }
    }

    public GAME_STATE GetCurrentGameState()//When called, return the current Game State
    {
        return currentGameState;
    }
}

public enum GAME_STATE //All possible Game States
{
    EXPLORATION,
    READING,
    PAUSE,
    CUTSCENES,
    FLASBACKS,
    DEAD
}
