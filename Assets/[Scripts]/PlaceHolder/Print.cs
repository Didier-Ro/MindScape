using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Print : MonoBehaviour
{
   public TextMeshProUGUI text;
   public WorldCondition si;
   private void Start()
   {
      text = GetComponent<TextMeshProUGUI>();
      si = GameManager.GetInstance().GetActualCondition();
      GameManager.GetInstance().OnConditionCompleted += ChangingCondition;
   }

   private void ChangingCondition(int condition)
   {
      string s = si.GetConditions();
      char charToRemove = '/';
      string resultString = s.Replace(charToRemove.ToString(), "");
      text.text = resultString;
   }
  

   private void Update()
   {
      
   }
}
