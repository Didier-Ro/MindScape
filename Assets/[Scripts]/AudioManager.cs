using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        selfAudioSource = GetComponent<AudioSource>();
    }

    [SerializeField] SoundLibrary soundLibrary;
    private AudioSource selfAudioSource;
    private GameObject prefabAudioSource;
    private List<AudioSource> audioSourcesList = new List<AudioSource>();
    private List<AudioSource> pausedAudioSources = new List<AudioSource>();

    private bool isPaused = false;

    public void SetSound(SOUND_TYPE _sound)
    {
        if (!isPaused)
        {
            selfAudioSource.PlayOneShot(soundLibrary.GetRandomSoundFromType(_sound));
        }
    }

    public void SetSound(SOUND_TYPE _sound, Vector3 _position)
    {
        if (!isPaused)
        {
            AudioSource audio = GetAudioSource();
            audio.transform.position = _position;
            audio.clip = soundLibrary.GetRandomSoundFromType(_sound);
            audio.Play();
        }
    }

    private AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSourcesList.Count; i++)
        {
            if (!audioSourcesList[i].isPlaying)
            {
                return audioSourcesList[i];
            }
        }

        AudioSource s = Instantiate(prefabAudioSource, transform).GetComponent<AudioSource>();
        audioSourcesList.Add(s);
        return s;
    }

    public void PauseAllSounds()
    {
        foreach (AudioSource audioSource in audioSourcesList)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                pausedAudioSources.Add(audioSource);
            }
        }
        selfAudioSource.Pause();
    }

    public void ResumeAllSounds()
    {
        foreach (AudioSource audioSource in pausedAudioSources)
        {
            audioSource.UnPause();
        }
        pausedAudioSources.Clear();
        selfAudioSource.UnPause();
    }

    private void Update()
    {
        if (GameManager.GetInstance().isPaused != isPaused)
        {
            isPaused = GameManager.GetInstance().isPaused;
            if (isPaused)
            {
                PauseAllSounds();
            }
            else
            {
                ResumeAllSounds();
            }
        }
    }
}

