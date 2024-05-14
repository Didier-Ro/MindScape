using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    #region SingeTone
    public static AudioManager GetInstance()
    {
        return _instance;
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        selfAudioSource = GetComponent<AudioSource>();
    }
    
    #endregion

    #region SubscribeToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and shows a different UI
    {
        switch (_newGameState)
        {
            case GAME_STATE.PAUSE:
                //StopSound();
                break;
            case GAME_STATE.EXPLORATION:
                //ResumeSound();
                break;
            case GAME_STATE.READING:
               
                break;
            case GAME_STATE.DEAD:
               
                break;
        }   
    }
    #endregion

    [SerializeField] SoundLibrary soundLibrary;
    AudioSource selfAudioSource;
    GameObject prefabAudioSource;
    List<AudioSource> audioSourcesList = new List<AudioSource>();


    public void SetSound(SOUND_TYPE _sound)
    {
        selfAudioSource.PlayOneShot(soundLibrary.GetRandomSoundFromType(_sound));
    }
    public void SetSound(SOUND_TYPE _sound, Vector3 _position)
    {
        AudioSource audio = GetAudioSource();

        audio.transform.position = _position;
        audio.clip = soundLibrary.GetRandomSoundFromType(_sound);
        audio.Play();
    }
    AudioSource GetAudioSource()
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
}

