using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLetter",menuName = "Letter")]
public class LetterScriptable : ScriptableObject
{
    [SerializeField] private string letterName = default;
    [SerializeField] private List<string> letterLines;
    [SerializeField] private string letterSign = default;

    public List<string> Lines
    {
        get { return letterLines; }
    }
}
