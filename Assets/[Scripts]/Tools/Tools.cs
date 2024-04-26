using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Tools : MonoBehaviour
{
   #if UNITY_EDITOR
       [MenuItem("MindScape tools/Reset Player Prefs in Game")]
      public static void ClearPlayerLevel()
       {
           GameManager gameManager = FindAnyObjectByType<GameManager>();
           if (gameManager == null)
           {
               EditorUtility.DisplayDialog(
                   "Player NOT FOUND",
                   "Proccess CANNOT BE COMPLETED",
                   "OK :("
                   );
           }
           else
           {
               gameManager.ResetPreferences();
               EditorUtility.DisplayDialog(
               "Player stats cleared",
               "Proccess done",
               "OK :)"
               );
           }
       }
   
       [MenuItem("MindScape tools/Reset Player in Main Menu ")]
       public static void ResetPlayerPrefs()
       {
           ChangeGame changeGame = FindAnyObjectByType<ChangeGame>();
           if (changeGame == null)
           {
               EditorUtility.DisplayDialog(
                   "Player NOT FOUND",
                   "Proccess CANNOT BE COMPLETED",
                   "OK :("
                   );
           }
           else
           {
               changeGame.ResetPlayerPrefs();
               EditorUtility.DisplayDialog(
               "Player stats cleared",
               "Proccess done",
               "OK :)"
               );
           }
       }
   #endif
}
