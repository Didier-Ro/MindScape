using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCutscene : MonoBehaviour
{
    [SerializeField] private Dialog dialog;
  

   public void ActivateChatbox()
    {
        Debug.Log("Holi");
        StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
        GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
    }

    public void DeactivateChatbox()
    {
        Debug.Log("Adioh");
        //letterManager.AddLetter(AddLetterToScriptableObject());
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        //GameManager.GetInstance().MarkConditionCompleted(conditionId);
        Destroy(gameObject);
    }
}
