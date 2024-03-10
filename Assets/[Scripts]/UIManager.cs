using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;
    public static UIManager GetInstance() 
    {
        return Instance;
    }

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject deadUI;

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
                PauseUI();
                break;
            case GAME_STATE.EXPLORATION:
                ExploringUI();
                break;
            case GAME_STATE.READING:
                ReadingUI();
                break;
            case GAME_STATE.DEAD:
                DeadUI();
                break;
        }   
    }

    public void InteractionUI() 
    {
        Debug.Log("Player Detected");
    }
    private void PauseUI() 
    {
        Debug.Log("Pause");
    }
    private void ExploringUI() 
    {
        Debug.Log("Exploring");
    }

    private void ReadingUI() 
    {
        //Enable Reading UI in hierarchy
    }
    private void DeadUI() 
    {

    }
}
