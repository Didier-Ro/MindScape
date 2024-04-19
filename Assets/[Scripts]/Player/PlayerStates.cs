using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    #region Singletone
    private static PlayerStates Instance;
    public static PlayerStates GetInstance()
    {
        return Instance;
    }
    #endregion

    private PLAYER_STATES currentPlayerState = PLAYER_STATES.PLAY;
    public Action<PLAYER_STATES> OnPlayerStateChanged;

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
    }

    private void Start()
    {
        ChangePlayerState(PLAYER_STATES.PLAY);
    }

    public void ChangePlayerState(PLAYER_STATES _newState)
    {
        currentPlayerState = _newState;

        if (OnPlayerStateChanged != null)
        {
            OnPlayerStateChanged.Invoke(currentPlayerState);
        }
    }

    public PLAYER_STATES GetCurrentPlayerState()
    {
        return currentPlayerState;
    }
}

public enum PLAYER_STATES
{
    PLAY,
    FALL,
}
