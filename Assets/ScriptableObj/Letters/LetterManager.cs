using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LetterManager",menuName = "Letter/LetterManager")]
public class LetterManager :ScriptableObject
{
   public List<LetterStructure> _discoveredLetters = new List<LetterStructure>();
   public void AddLetter(LetterStructure scriptableLetter)
   {
      _discoveredLetters.Add(scriptableLetter);
      EditorUtility.SetDirty(this);
      AssetDatabase.SaveAssets();
   }
   
   public List<LetterStructure> GetLetterList()
   {
      return _discoveredLetters;
   }

   public void ResetLetters()
   {
      _discoveredLetters.Clear();
      EditorUtility.SetDirty(this);
      AssetDatabase.SaveAssets();
   }
}
[Serializable]
public class LetterStructure
{
   public string letterTittle;
   public List<string> letterBody;
}

