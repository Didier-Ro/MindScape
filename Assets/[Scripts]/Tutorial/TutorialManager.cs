using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Singletone
    private static TutorialManager instance;
    public static TutorialManager GetInstance()
    {
        return instance;
    }
    #endregion

    [SerializeField] private TutorialSO tutorialSO;
    [SerializeField] private TextMeshProUGUI tutorialText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorial(Tutorial_Type _tutorialType)
    {
        switch (_tutorialType)
        {
            case Tutorial_Type.MOVEMENT:
                tutorialText.text = tutorialSO.tutorials[0].tutorialText;
                tutorialText.enabled = true;
                break;
        }
    }

    public void TutorialDone(Tutorial_Type _tutorialType)
    {
        switch (_tutorialType)
        {
            case Tutorial_Type.MOVEMENT:
                tutorialSO.tutorials[0].isDone = true;
                tutorialText.enabled = false;
                break;
        }
    }

    public bool TutorialIsDone(Tutorial_Type _tutorialType)
    {
        bool tutorialDone = false;
        switch ( _tutorialType)
        {
            case Tutorial_Type.MOVEMENT:
                tutorialDone = tutorialSO.tutorials[0].isDone;
                break;
        }
        return tutorialDone;
    }
}
