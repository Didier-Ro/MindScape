using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private InputReturnValue inputValue;
    [SerializeField] private GameObject UIAnimator;
    private Animator _animator;
    private void FixedUpdate()
    {
        switch (inputValue)
        {
            case InputReturnValue.FOCUS:
                InputManager.GetInstance().playerControls.Gameplay.FocusNext.Enable();
                if (InputManager.GetInstance().FocusNextGoal())
                {
                    UIAnimator.SetActive(false);
                   _animator.SetBool("NorthButton", false);
                    GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
                    Destroy(gameObject);
                }
                break;
            case InputReturnValue.CHANGING_LIGHT:
                if (InputManager.GetInstance().FlashligthInput())
                {
                    UIAnimator.SetActive(false);
                    _animator.SetBool("PressRT", false);
                    GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
                    Destroy(gameObject);
                }
                break;
        }
    }
    
    
    private void OnEnable()
    {
        UIAnimator.SetActive(true);
        _animator = UIAnimator.GetComponent<Animator>();
        if (inputValue == InputReturnValue.FOCUS)
        {
            _animator.SetBool("NorthButton", true);
            GameManager.GetInstance().ChangeGameState(GAME_STATE.CUTSCENES);
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.TUTORIAL);
        }
        else if (inputValue == InputReturnValue.CHANGING_LIGHT)
        {
            _animator.SetBool("PressRT", true);
            GameManager.GetInstance().ChangeGameState(GAME_STATE.TUTORIAL);
        }
    }
    

    enum InputReturnValue
    {
        FOCUS,
        CHANGING_LIGHT
    }
}
