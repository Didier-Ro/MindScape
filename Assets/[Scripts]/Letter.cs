using System;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Assets.SimpleLocalization.Scripts
{
    public class Letter : MonoBehaviour, Istepable
    {
        [SerializeField] private Dialog dialog;
        [SerializeField] private GameObject interactionCanvas = default;
        [SerializeField] private GameObject keyboardUI = default;
        [SerializeField] private GameObject gamepadUI = default;
        public String keyTittle;
        public List<String> localizationKey = new List<string>();
        [SerializeField] private LetterManager letterManager;
        private string currentControlScheme;

        private void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag== "Player")
            {
                SetActiveCanvas();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag== "Player")
            {
                DeactivateCanvas();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag== "Player")
            {
                SetActiveCanvas();
            }
        }

        private void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
            Debug.Log(letterManager.GetLetterList().Count);
        }

        public void Activate()
        {
            StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
            GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
        }

        public void Deactivate()
        {
            letterManager.AddLetter(AddLetterToScriptableObject());
            GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
            Destroy(gameObject);
        }

        private LetterStructure AddLetterToScriptableObject()
        {
            LetterStructure letterStructure = new LetterStructure();
            letterStructure.letterTittle = keyTittle;
            letterStructure.letterBody = new List<string>();
            foreach (var body in localizationKey)
            {
                letterStructure.letterBody.Add(body);
            }
            return letterStructure;
        }
        
        private void Localize()
        {
            dialog.Lines.Clear();
            foreach (var key in localizationKey)
            {
                dialog.Lines.Add(LocalizationManager.Localize(key));
            }
        }

        public void SetActiveCanvas()
        {
            if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
            {
                interactionCanvas.SetActive(true);
                keyboardUI.SetActive(false);
                gamepadUI.SetActive(true);
            }
            else if(InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
            {
                interactionCanvas.SetActive(true);
                gamepadUI.SetActive(false);
                keyboardUI.SetActive(true);
            }
        }

        public void DeactivateCanvas()
        {
            interactionCanvas.SetActive(false);
        }
    }
}
