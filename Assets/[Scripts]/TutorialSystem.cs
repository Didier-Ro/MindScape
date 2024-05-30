using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private InputReturnValue inputValue;
    [SerializeField] [CanBeNull] private GameObject UI;
    private void FixedUpdate()
    {
        switch (inputValue)
        {
            case InputReturnValue.FOCUS:
                InputManager.GetInstance().playerControls.Gameplay.FocusNext.Enable();
                if (InputManager.GetInstance().FocusNextGoal())
                {
                    GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
                    Destroy(gameObject);
                   
                }
                break;
            case InputReturnValue.CHANGING_LIGHT:
                InputManager.GetInstance().playerControls.Gameplay.Protect.Enable();
                if (InputManager.GetInstance().FlashligthInput())
                {
                    GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
                    PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
                    Destroy(gameObject);
                }
                break;
        }
    }
    
    
    private void OnEnable()
    {
        if (inputValue == InputReturnValue.FOCUS)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.CUTSCENES);
        }
        else if (inputValue == InputReturnValue.CHANGING_LIGHT)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.TUTORIAL);
        }
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.TUTORIAL);
    }
    

    enum InputReturnValue
    {
        FOCUS,
        CHANGING_LIGHT
    }
}
