using Assets.SimpleLocalization.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{ 
  [SerializeField] private List<GameObject> letters = new List<GameObject>();
 
  [SerializeField] private List<int> conditions = new List<int>();
  
  private void OnEnable()
  {
      ConditionCheck();
  }

  private void ConditionCheck()
  {
    for (int i = 0; i < conditions.Count; i++)
    { 
        letters[i].SetActive(GameManager.GetInstance().IsConditionCompleted(conditions[i]));
    }
  }

 
}
