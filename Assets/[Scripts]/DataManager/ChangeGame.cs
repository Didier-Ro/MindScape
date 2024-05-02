using System;
using UnityEngine;

public class ChangeGame : MonoBehaviour
{
   [SerializeField] private WorldCondition stageConditions;
   [SerializeField] private WorldCondition[] allConditions;
   [SerializeField] private string[] levelScenes;
   [SerializeField] private int[] conditionsIds;
   public int[] currentLevel;
   public float[] percentageOfGameCompleted;

   private void Awake()
   { 
      ResetAll();
      LoadAllData();
      GetCurrentLevel();
      GetPercentageOfAllGamesCompleted();
   }

    private void GetPercentageOfAllGamesCompleted()
   {
      for (int i = 0; i < allConditions.Length; i++)
      {
         percentageOfGameCompleted[i] = allConditions[i].GetPercentageOfGameCompleted();
      }
   }

   public void ResetPlayerPrefs()
   {
         PlayerPrefs.DeleteAll();
   }

   private void ResetAll()
   {
      for (int i = 0; i < allConditions.Length; i++)
      {
         allConditions[i].ResetData();
      }
   }

   private void RestartANewGameData(int _gameToRestart)
   {
      foreach (var stageCondition in allConditions)
      {
         if (stageCondition.nGame == _gameToRestart)
         {
            stageCondition.RestartDataToANewGame();
         }
      }
      SaveAllData();
   }

   private string RestartAllGamesToNewGames()
   {
      ResetAll();
      for (int i = 0; i <allConditions.Length; i++)
      {
         allConditions[i].nGame = i + 1;
      }
      string dataToSave = "";
      for (int i = 0; i < allConditions.Length; i++)
      {
         dataToSave += allConditions[i].RestartDataToANewGame() + "*";
      }
      return dataToSave;
   }
   
   private void SaveAllData()
   {
      string dataToSave = "";
      for (int i = 0; i < allConditions.Length; i++)
      {
         dataToSave += allConditions[i].SaveData() + "*";
      }
      PlayerPrefs.SetString("alldata", dataToSave);
   }
   private void LoadAllData()
   {
      string[] dataToLoad = PlayerPrefs.GetString("alldata",RestartAllGamesToNewGames()).Split("*");
      for (int i = 0; i < allConditions.Length; i++)
      {
         allConditions[i].LoadData(dataToLoad[i]);
      }
   }
   
   private void LoadCurrentGameData(int _currentGame)
   {
      foreach (var stageCondition in allConditions)
      {
         if (stageCondition.nGame == _currentGame)
         {
            stageConditions = stageCondition.GetNumOfGame();
         }
      }
   }

   private void GetCurrentLevel()
   {
      for (int i = 0; i < allConditions.Length; i++)
      {
         if (allConditions[i].IsFirstTimePlayed())
         {
            currentLevel[i] = 0;
         }
         else
         {
               for (int index = 0; index < conditionsIds.Length; index++)
               {
                  if (allConditions[i].IsConditionCompleted(conditionsIds[index]))
                  {
                     currentLevel[index] = index + 1;
                  }
               }
         }
      }
     
         
         
         
   }
   public void SelectGame(int _num)
   {
      PlayerPrefs.SetInt("GameNumber", _num);
      LoadCurrentGameData(_num);
      LoadingManager.instance.LoadScene(levelScenes[currentLevel[_num-1]]);
      if (currentLevel[_num-1] == 0)
      {
         SaveAllData();
      }
   }
}
