using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, Fstepable
{
    [SerializeField] private AudioClip[] audioClips;
    private int index = 0;

    public void FActivate()
    {
        index = Random.Range(0, audioClips.Length);
        AudioManager.GetInstance().PlaySFX(audioClips[index]);
    }

    public void FDeactivate()
    {
        
    }
}
