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
                    gameObject.SetActive(false);
                }
                break;
            case InputReturnValue.CHANGING_LIGHT:
                InputManager.GetInstance().playerControls.Gameplay.Protect.Enable();
                if (InputManager.GetInstance().FlashligthInput())
                {
                    GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
                    PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
                    gameObject.SetActive(false);
                }
                break;
        }
    }


    private void OnEnable()
    {
        GameManager.GetInstance().ChangeGameState(GAME_STATE.CUTSCENES);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.TUTORIAL);
    }
    

    enum InputReturnValue
    {
        DASHING,
        FOCUS,
        CHANGING_LIGHT
    }
}
