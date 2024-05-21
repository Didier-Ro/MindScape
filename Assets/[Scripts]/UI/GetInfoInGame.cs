using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetInfoInGame : MonoBehaviour
{
    [SerializeField] private int index;
    public TMP_Text percentageCompletedText;
    public TMP_Text currentLevelText;
    public TMP_Text currentTimePlayedText;


    private void OnEnable()
    {
       GetInfoInText();
    }

    private void GetInfoInText()
    {
        GameManager.GetInstance().GetPercentageOfAllGamesCompleted();
        GameManager.GetInstance().GetCurrentLevel();
        GameManager.GetInstance().ReturnTimePlayed();
        
        int percentageCompleted =  GameManager.GetInstance().percentageOfGameCompleted[index];
        percentageCompletedText.text = "Completed: " + percentageCompleted;
        
        int currentLevel =  GameManager.GetInstance().percentageOfGameCompleted[index];
        percentageCompletedText.text = "Current Level: " + currentLevel;
        
        int time = GameManager.GetInstance().gamesTimePlayed[index];
        int minutes = time / 60;
        int seconds = time % 60;
        currentTimePlayedText.text = minutes + "m " + seconds + "s";
    }
}
