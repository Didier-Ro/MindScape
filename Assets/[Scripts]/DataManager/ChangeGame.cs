using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGame : MonoBehaviour
{
   public void SelectGame(int _num)
   {
      PlayerPrefs.SetInt("GameNumber", _num);
   }
}
