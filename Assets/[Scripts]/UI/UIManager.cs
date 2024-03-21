using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;
    public static UIManager GetInstance() 
    {
        return Instance;
    }
    [SerializeField] private EventSystem eventSystem = default;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject deadUI;
    [SerializeField] private TabGroup tabGroup;

    [SerializeField] private GameObject deadButtonUI;
    [SerializeField] private LetterScriptable dialog;

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
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
        }
    }
    
    public void ChangeUISelected(GameObject objectToSelect)
    {
       eventSystem.SetSelectedGameObject(objectToSelect);
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
        pauseUI.SetActive(true);
        tabGroup.ResetAll();
    }
    private void ExploringUI() 
    {
       pauseUI.SetActive(false);
    }

    private void ReadingUI() 
    {
        //Enable Reading UI in hierarchy
       DialogManager.GetInstance().ShowDialog(dialog);
    }
    private void DeadUI() 
    {
        deadUI.SetActive(true);
        ChangeUISelected(deadButtonUI);
    }
}
