using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;

public class DialogCutscene : MonoBehaviour
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

    [SerializeField] private Dialog dialog;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject text;
    [SerializeField] private TextMeshProUGUI textColor;
    [SerializeField] private GameObject[] gameUI;

    public PlayableDirector director;
    private bool chatboxActive = false;
    private string currentControlScheme;

    private void Start()
    {
        SubscribeToGameManagerGameState();
        GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
    }

    private void Update()
    {
        if (chatboxActive)
        {
            InputManager.GetInstance().playerControls.Reading.Enable();
            if (InputManager.GetInstance().NextInput())
            {
                background.SetActive(false);
                textColor.text = "";
                director.Play();
                chatboxActive = false;
                DeactivateCanvas();
            }
        }
    }

    public void ActivateChatbox()
    {
        dialog.Localize();
        StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
    }

    public void DeactivateChatbox()
    {
        director.Pause();
        chatboxActive = true;
        SetActiveCanvas();
    }

    public void SetActiveCanvas()
    {
        if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
        {
            gameUI[0].SetActive(false);
            gameUI[1].SetActive(true);
        }
        else if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
        {
            gameUI[0].SetActive(true);
            gameUI[1].SetActive(false);
        }
    }

    public void DeactivateCanvas()
    {
        gameUI[0].SetActive(false);
        gameUI[1].SetActive(false);
    }
}
