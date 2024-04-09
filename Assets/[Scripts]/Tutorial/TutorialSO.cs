using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial", menuName = "ScriptableObjects/Tutorial", order = 0)]
public class TutorialSO : ScriptableObject
{
    public Tutorials[] tutorials;
}

[Serializable]
public struct Tutorials
{
    public Tutorial_Type type;
    public string tutorialText;
    public bool isDone;

}