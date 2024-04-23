using UnityEngine;
using System;
using Assets.SimpleLocalization.Scripts;
using TMPro;
namespace Assets.SimpleLocalization
{
    public class LocalizeManager : MonoBehaviour
    {
        public static LocalizeManager instance;
        public TextMeshProUGUI FormattedText;

        public LocalizeManager GetInstance()
        {
            return instance;
        }
        /// <summary>
        /// Called on app start.
        /// </summary>
        public void Awake()
        {
            LocalizationManager.Read();
            switch (PlayerPrefs.GetString("Language", Application.systemLanguage.ToString()))
            {
                case "Spanish":
                    LocalizationManager.Language = "Spanish";
                    break;
                case "English":
                    LocalizationManager.Language = "English";
                    break;
                default:
                    LocalizationManager.Language = "English";
                    break;
            }
Debug.Log(Application.systemLanguage.ToString());
            // This way you can localize and format strings from code.
            FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);

            // This way you can subscribe to LocalizationChanged event.
            LocalizationManager.OnLocalizationChanged += () => FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);
        }

        /// <summary>
        /// Change localization at runtime.
        /// </summary>
        public void SetLocalization(string localization)
        {
            LocalizationManager.Language = localization;
            PlayerPrefs.SetString("Language", localization);
        }
        

        /// <summary>
        /// Write a review.
        /// </summary>
        public void Review()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/120113");
        }
    }
}   
