using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;

public class CreateLettersUI : MonoBehaviour
{
    [SerializeField] private GameObject letter;
    public void CreateLetter(LetterStructure letterStructure)
    {
        GameObject Letter =  Instantiate(letter, gameObject.transform);
        LocalizedTextMeshPro localize = Letter.GetComponentInChildren<LocalizedTextMeshPro>();
        localize.LocalizationKey = letterStructure.letterTittle;
        localize.enabled = true;
    }
}
