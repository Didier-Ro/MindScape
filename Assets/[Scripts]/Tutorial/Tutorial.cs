using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Tutorial_Type tutorial;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!TutorialManager.GetInstance().TutorialIsDone(tutorial))
            {
                TutorialManager.GetInstance().ShowTutorial(tutorial);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TutorialManager.GetInstance().TutorialDone(tutorial);
    }

}

public enum Tutorial_Type
{
    MOVEMENT,
    LIGHT
}