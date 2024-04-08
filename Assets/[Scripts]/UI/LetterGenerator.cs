using Assets.SimpleLocalization.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{ 
   [SerializeField] private GameObject letter;
  [SerializeField] private LetterManager letterManager;
  private void OnEnable()
  {
     SpawnLetters();
  }

  private List<LetterStructure> GetLetters()
  {
    List<LetterStructure> letterStructures = new  List<LetterStructure>();
    foreach (var letter in  letterManager.GetLetterList())
    {
        letterStructures.Add(letter);
    }
    return letterStructures;
  }

  private void SpawnLetters()
  {
      for (int i = 0; i < GetLetters().Count; i++)
      {
          CreateLetter(GetLetters()[i]);
      }
  }
  
  public void CreateLetter(LetterStructure letterStructure)
  {
      GameObject Letter =  Instantiate(letter, gameObject.transform);
      LocalizedTextMeshPro localize = Letter.GetComponentInChildren<LocalizedTextMeshPro>();
      localize.LocalizationKey = letterStructure.letterTittle;
      localize.enabled = true;
  }
}
