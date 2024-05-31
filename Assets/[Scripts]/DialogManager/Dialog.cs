using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleLocalization.Scripts;

[System.Serializable]
public class Dialog
{
   [SerializeField] private List<string> lines;
   [SerializeField] private LocalizedTextMeshProArray localize;

   public List<string> Lines
   {
      get { return lines; }
   }

    public void Localize()
    {
        lines = localize.texts;
    }
}
