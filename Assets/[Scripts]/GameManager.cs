using System;
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
        ResetAll();
        LoadAllData();
    }

    private void OnDestroy()
    {
        SaveAllData();
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

    #region WorldConditions

    [SerializeField] private WorldCondition stageConditions;
    [SerializeField] private WorldCondition[] allConditions;
    public Action<int> OnConditionCompleted;

    public void MarkConditionCompleted(int _i)
    {
        stageConditions.MarkCondition(_i);
        if (OnConditionCompleted != null)
        {
            OnConditionCompleted(_i);
        }
    }
    
    public bool IsConditionCompleted(int _id)
    {
        return stageConditions.IsConditionCompleted(_id);
    }
    
    private void SaveAllData()
    {
        string dataToSave = "";
        for (int i = 0; i < allConditions.Length; i++)
        {
            dataToSave += allConditions[i].SaveData() + "*";
        }
        PlayerPrefs.SetString("alldata", dataToSave);
    }

    private void ResetAll()
    {
        for (int i = 0; i < allConditions.Length; i++)
        {
            allConditions[i].ResetData();
        }
    }
    
    public void SelectGameNum(int _number)
    {
        PlayerPrefs.SetInt("GameNumber", _number);
    }

    private void LoadAllData()
    {
        string[] dataToLoad = PlayerPrefs.GetString("alldata").Split("*");
        for (int i = 0; i < allConditions.Length; i++)
        {
            allConditions[i].LoadData(dataToLoad[i]);
        }
        LoadCurrentGameData(PlayerPrefs.GetInt("GameNumber", 1));
       // CheckpointManager.AddCheckpointPosition(stageConditions.lastPosition);
    }

    public void SaveSpecificGameData(int _gameUWantToReplace)
    {
        foreach (var stageCondition in allConditions)
        {
            if (stageCondition.nGame == _gameUWantToReplace)
            {
                stageCondition.ResetData();
                stageCondition.LoadData(stageConditions.GetData(_gameUWantToReplace));
            }
        }
        SaveAllData();
    }
    private void LoadCurrentGameData(int _currentGame)
    {
        foreach (var stageCondition in allConditions)
        {
            if (stageCondition.nGame == _currentGame)
            {
                stageConditions = stageCondition.GetNumOfGame();
            }
        }
    }
    
    #endregion
    
}

public enum GAME_STATE //All possible Game States
{
    EXPLORATION,
    READING,
    PAUSE,
    CUTSCENES,
    FLASBACKS,
    FALLING,
    DEAD
}
