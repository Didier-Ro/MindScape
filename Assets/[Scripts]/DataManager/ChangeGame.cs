using System;
using UnityEngine;

public class ChangeGame : MonoBehaviour
{
   [SerializeField] private WorldCondition stageConditions;
   [SerializeField] private WorldCondition[] allConditions;
   [SerializeField] private string[] levelScenes;
   [SerializeField] private string animationScene;
   [SerializeField] private int[] conditionsIds;
   [SerializeField] private string defaultdata;

   

   private void Start()
   {
      ResetAll();
      LoadAllData();
   }

   private void ResetAll()
   {
      for (int i = 0; i < allConditions.Length; i++)
      {
         allConditions[i].ResetData();
      }
   }
   
   
   private void LoadAllData()
   {
      PlayerPrefs.SetString("alldata",defaultdata);
      string[] dataToLoad = PlayerPrefs.GetString("alldata",defaultdata).Split("*");
      for (int i = 0; i < allConditions.Length; i++)
      {
         allConditions[i].LoadData(dataToLoad[i]);
      }
      for (int i = 0; i < dataToLoad.Length; i++)
      {
         Debug.Log(dataToLoad[i]); // Print each element individually
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
   public void SelectGame(int _num)
   {
      PlayerPrefs.SetInt("GameNumber", _num);
      LoadCurrentGameData(_num);
      string sceneToPlay = default;
      if (stageConditions.IsFirstTimePlayed())
      {
         sceneToPlay = animationScene;
      }
      else
      {
         for (int i = 0; i < conditionsIds.Length; i++)
         {
            if (stageConditions.IsConditionCompleted(conditionsIds[i]))
            {
               sceneToPlay = levelScenes[i];
            }
         }
      }
      LoadingManager.instance.LoadScene(sceneToPlay);
   }
}
