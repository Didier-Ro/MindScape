using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    public float masterVolumeSet = 1;
    public float sfxVolumeSet = 1;
    public float musicVolumeSet = 1;

    public void ChangeSFXVolume()
    {
        sfxVolumeSet = sfxSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(sfxVolumeSet)) * -80f;
        mixer.SetFloat("SFX_Vol", volumeInDB);
        PlayerPrefs.SetFloat("SFX_Vol", sfxVolumeSet);
    }

    public void ChangeMasterVolume()
    {
        masterVolumeSet = masterSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(masterVolumeSet)) * -80f;
        mixer.SetFloat("Master_Vol", volumeInDB);
        PlayerPrefs.SetFloat("Master_Vol", masterVolumeSet);
    }

    public void ChangeMusicVolume()
    {
        musicVolumeSet = musicSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(musicVolumeSet)) * -80f;
        mixer.SetFloat("Music_Vol", volumeInDB);
        PlayerPrefs.SetFloat("Music_Vol", musicVolumeSet);
    }
}
