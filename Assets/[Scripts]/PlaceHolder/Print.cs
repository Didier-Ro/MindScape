using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Print : MonoBehaviour
{
   public TextMeshProUGUI text;

   private void Start()
   {
      text = GetComponent<TextMeshProUGUI>();
   }

   private void Update()
   {
     // text.text = GameManager.GetInstance().
   }
}
