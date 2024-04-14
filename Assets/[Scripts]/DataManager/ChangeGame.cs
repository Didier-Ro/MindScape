using UnityEngine;

public class ChangeGame : MonoBehaviour
{
   [SerializeField] private WorldCondition stageConditions;
   [SerializeField] private WorldCondition[] allConditions;
   [SerializeField] private string gameScene;
   [SerializeField] private string animationScene;
   
   
   private void ResetAll()
   {
      for (int i = 0; i < allConditions.Length; i++)
      {
         allConditions[i].ResetData();
      }
   }
   
   private void LoadAllData()
   {
      string[] dataToLoad = PlayerPrefs.GetString("alldata").Split("*");
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
   public void SelectGame(int _num)
   {
      ResetAll();
      LoadAllData();
      PlayerPrefs.SetInt("GameNumber", _num);
      LoadCurrentGameData(_num);
      string sceneToPlay = stageConditions.IsFirstTimePlayed() ? animationScene : gameScene; 
      LoadingManager.instance.LoadScene(sceneToPlay);
   }
}
