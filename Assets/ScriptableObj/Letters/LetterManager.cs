#if UNITY_EDITOR
using UnityEditor;
#endif
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
      #if UNITY_EDITOR
      EditorUtility.SetDirty(this);
      #endif
      AssetDatabase.SaveAssets();
   }
   
   public List<LetterStructure> GetLetterList()
   {
      return _discoveredLetters;
   }

   public void ResetLetters()
   {
      _discoveredLetters.Clear();
      #if UNITY_EDITOR
      EditorUtility.SetDirty(this);
      #endif
      AssetDatabase.SaveAssets();
   }
}
[Serializable]
public class LetterStructure
{
   public string letterTittle;
   public List<string> letterBody;
}

