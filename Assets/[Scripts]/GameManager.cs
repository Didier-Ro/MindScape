using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager Instance;
    public static GameManager GetInstance() 
    {
        return Instance;
    }
    #endregion
    [SerializeField] private float minuteQuickSaveRate;
    private bool isPaused;
    private bool isFlashing;
    private int framesPlayed;
    private GAME_STATE currentGameState = GAME_STATE.EXPLORATION;
    public Action<GAME_STATE> OnGameStateChange;
    public Action<bool> OnFlashingChange;

    [Header("Shadow References")]
    [SerializeField] private Light2D flashlightReference;
    [SerializeField] private Material shadowMaterialReference;

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
    
    private void Update()
    {
        if (InputManager.GetInstance().FlashligthInput())
        {
           ToggleFlash();
           OnFlashingChange.Invoke(isFlashing);
        }
        if (InputManager.GetInstance().SetPause())
        {
            GAME_STATE actualGameState = TogglePause() ? GAME_STATE.PAUSE : GAME_STATE.EXPLORATION;
            ChangeGameState(actualGameState);
        }
    }

    private void FixedUpdate()
    {
        if (GetCurrentGameState() != GAME_STATE.PAUSE && framesPlayed <= minuteQuickSaveRate * 3600)
        {
            framesPlayed++;
        }
        else if(GetCurrentGameState() != GAME_STATE.PAUSE && framesPlayed >= minuteQuickSaveRate * 3600)
        { 
           Debug.Log("guardado automatico se ha hecho");
           framesPlayed++;
           SaveAllData();
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

    public void ResetPreferences()
    {
        PlayerPrefs.DeleteAll();
    }

    public GAME_STATE GetCurrentGameState()//When called, return the current Game State
    {
        return currentGameState;
    }

    public Light2D GetLightReference()
    {
        return flashlightReference;
    }

    public Material GetShadowMaterial()
    {
        return shadowMaterialReference;
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

    public void SavePlayerPosition(Vector3 position)
    {
        stageConditions.SavePlayerPosition(position);
    }
    
    public bool IsConditionCompleted(int _id)
    {
        return stageConditions.IsConditionCompleted(_id);
    }
    
    private void SaveAllData()
    {
        stageConditions.AddSecondsToTheTimePlayed(framesPlayed);
        framesPlayed = 0;
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
    
    
    private string RestartAllGamesToNewGames()
    {
        ResetAll();
        for (int i = 0; i <allConditions.Length; i++)
        {
            allConditions[i].nGame = i + 1;
        }
        string dataToSave = "";
        for (int i = 0; i < allConditions.Length; i++)
        {
            dataToSave += allConditions[i].RestartDataToANewGame() + "*";
        }
        return dataToSave;
    }

    private void LoadAllData()
    {
        string[] dataToLoad = PlayerPrefs.GetString("alldata", RestartAllGamesToNewGames()).Split("*");
        for (int i = 0; i < allConditions.Length; i++)
        {
            allConditions[i].LoadData(dataToLoad[i]);
        }
        LoadCurrentGameData(PlayerPrefs.GetInt("GameNumber", 1));
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
        Debug.Log(stageConditions.lastPosition);
        CheckpointManager.AddCheckpointPosition(stageConditions.lastPosition);
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
