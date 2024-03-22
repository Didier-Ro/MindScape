using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{
  [SerializeField] private LetterManager letterManager;
  [SerializeField] private CreateLettersUI createLettersUI;
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
          createLettersUI.CreateLetter(GetLetters()[i]);
      }
  }
}
