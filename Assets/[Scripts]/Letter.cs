using System;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.SimpleLocalization.Scripts
{
    public class Letter : MonoBehaviour, Istepable
    {
        [SerializeField] private Dialog dialog;
        public String keyTittle;
        public List<String> localizationKey = new List<string>();
        [SerializeField] private LetterManager letterManager;

        private void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
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
            foreach (var key in localizationKey)
            {
                dialog.Lines.Add(LocalizationManager.Localize(key));
            }
        }
    }
}
