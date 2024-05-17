using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Scriptables/SoundLibrary", order = 1)]
public class SoundLibrary : ScriptableObject
{
    [SerializeField] SoundArchive[] soundArchives;

    public AudioClip GetRandomSoundFromType(SOUND_TYPE _st)
    {
        for (int i = 0; i < soundArchives.Length; i++)
        {
            if (soundArchives[i].soundType == _st)
            {
                int randomnum = UnityEngine.Random.Range(0, soundArchives[i].sounds.Length);
                return soundArchives[i].sounds[randomnum];
            }
        }
        return null;
    }
}
[Serializable]
public class SoundArchive
{
    public SOUND_TYPE soundType;
    public AudioClip[] sounds;
}
public enum SOUND_TYPE
{
    PASOS_CONCRETO,
    PASOS_MADERA,
    FUEGO = 3,
    GOLPE,
    MUERTE_ENEMIGO,
    PASOS_ALFOMBRA = 2,
    GOLPE_ENEMIGO,
}