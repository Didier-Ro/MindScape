using System;
using TMPro;
using UnityEngine;

public class Print : MonoBehaviour
{
   private TextMeshProUGUI text;
   public bool GameManagerExist = true;
   private WorldCondition si;

   private void Awake()
   {
      text = GetComponent<TextMeshProUGUI>();
   }

   private void Start()
   {
      if (GameManagerExist)
      {
         Debug.Log("funciona por favor");
         si = GameManager.GetInstance().GetActualCondition();
         GameManager.GetInstance().OnConditionCompleted += ChangingCondition;
         ChangingCondition(0);
      }
   }

   public void MostrarElTexto(string texto)
   {
      text.text = texto;
   }

   private void ChangingCondition(int condition)
   {
      string s = si.GetConditions();
      char charToRemove = '/';
      string resultString = s.Replace(charToRemove.ToString(), "");
      text.text = resultString;
   }
  
   
}
