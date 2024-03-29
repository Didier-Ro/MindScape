using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorSounds", menuName = "ScriptableObjects/FloorSounds", order = 0)]
public class FloorSoundsSO : ScriptableObject
{
    [SerializeField] public SOUNDS[] typeSounds;
}

[Serializable]
public struct SOUNDS
{
    public string materialSound;
    public AudioClip[] sound;
}
