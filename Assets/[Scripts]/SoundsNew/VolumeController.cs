using System;
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

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master_Vol", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Vol", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("Music_Vol", 0.5f);
        ChangeSFXVolume();
        ChangeMasterVolume();
        ChangeMusicVolume();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("SFX_Vol", sfxVolumeSet);
        PlayerPrefs.SetFloat("Master_Vol", masterVolumeSet);
        PlayerPrefs.SetFloat("Music_Vol", musicVolumeSet);
    }

    public void ChangeSFXVolume()
    {
        sfxVolumeSet = sfxSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(sfxVolumeSet)) * -80f;
        mixer.SetFloat("SFX_Vol", volumeInDB);
    }

    public void ChangeMasterVolume()
    {
        masterVolumeSet = masterSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(masterVolumeSet)) * -80f;
        mixer.SetFloat("Master_Vol", volumeInDB);
     
    }

    public void ChangeMusicVolume()
    {
        musicVolumeSet = musicSlider.value;
        float volumeInDB = (1 - Mathf.Sqrt(musicVolumeSet)) * -80f;
        mixer.SetFloat("Music_Vol", volumeInDB);
    }
}
