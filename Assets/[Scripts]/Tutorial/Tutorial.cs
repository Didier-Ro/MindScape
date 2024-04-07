using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Tutorial_Type tutorial;
    /*[SerializeField] private string tutorialPhrase;
    TextMeshProUGUI tutorialText;
    GameObject obj;*/

    void Start()
    {
        /*obj = GameObject.Find("TutorialText");
        tutorialText = obj.GetComponent<TextMeshProUGUI>();*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!TutorialManager.GetInstance().TutorialIsDone(tutorial))
            {
                TutorialManager.GetInstance().ShowTutorial(tutorial);
            }
           /* tutorialText.text = tutorialPhrase;
            tutorialText.enabled = true;
            Debug.Log("Tutorial");*/
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TutorialManager.GetInstance().TutorialDone(tutorial);
        /*tutorialText.enabled = false;
        gameObject.SetActive(false);*/
    }

}

public enum Tutorial_Type
{
    MOVEMENT,
    LIGHT
}