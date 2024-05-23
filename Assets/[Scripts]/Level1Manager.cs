using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    #region SubscribeToGameManager
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
                //StopSound();
                break;
            case GAME_STATE.EXPLORATION:
                //ResumeSound();
                break;
            case GAME_STATE.READING:

                break;
            case GAME_STATE.DEAD:

                break;
        }
    }
    #endregion

    public GameObject player;

    void Start()
    {
        SubscribeToGameManagerGameState();
        GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DASHING);
        player.SetActive(false);
    }

    private void Update()
    {
        Debug.Log(GameManager.GetInstance().GetCurrentGameState());
    }

    public void ChangeGameStateInTimeline()
    {
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        player.SetActive(true);
    }
}
