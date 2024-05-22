using System;
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
    ROT_CAJAS_IMPACTO,
    RAYO_DE_LUZ_RELOADED,
    ROT_SUSURROS_ENEMIGO,
    ROT_PLACA_DE_PRESION,
    RAYO_DE_LUZ,
    CAJA_ARRASTRANDOSE,
    PUERTA_PIEDRA_ABRIENDOSE,
    PUERTA_PIEDRA_CERRANDOSE,
    LAMP_OFF,
    LAMP_JIGGLE,
    LAMP_ON,
    ORBE_DE_CRISTAL,
}