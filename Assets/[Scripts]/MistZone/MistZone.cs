using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistZone : MonoBehaviour
{
    [SerializeField] private float damagePerSecond;
    [SerializeField] private GameObject playerRef;
    private Flashlight flashLight;
    private HealthController healthController;
    private bool playerInZone;
    private bool canDamage;
    
    [SerializeField] private SoundLibrary soundLibrary;
    private AudioSource mistAudioSource;
    
    private AudioSource musicAudioSource;
    [SerializeField] private float musicVolumeInMist = 0.2f;
    private float originalMusicVolume;

    private void Start()
    {
        playerRef = GameObject.Find("Player");
        if (playerRef != null)
        {
            flashLight = playerRef.GetComponent<Flashlight>();
            healthController = playerRef.GetComponent<HealthController>();
        }
        
        mistAudioSource = GetComponent<AudioSource>();
        if (mistAudioSource == null)
        {
            mistAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        GameObject musicObject = GameObject.Find("Musica");
        if (musicObject != null)
        {
            musicAudioSource = musicObject.GetComponent<AudioSource>();
            originalMusicVolume = musicAudioSource.volume;
        }
    }

    private void Update()
    {
        if (playerInZone && flashLight.currentSliderValue <= 0)
        {
            canDamage = true;
        }
        else
        {
            canDamage = false;
        }

        if (playerInZone && !mistAudioSource.isPlaying)
        {
            PlayMistSound();
        }
    }

    private void FixedUpdate()
    {
        if (canDamage)
        {
            MakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
            PlayMistSound();
            ReduceMusicVolume();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
            StopMistSound();
            RestoreMusicVolume();
        }
    }

    private void MakeDamage()
    {
        float totalDamage = damagePerSecond / 60;
        healthController.PlayerTakeDamage(totalDamage);
    }

    private void PlayMistSound()
    {
        AudioClip mistSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.Neblina);
        if (mistSound != null)
        {
            mistAudioSource.clip = mistSound;
            mistAudioSource.loop = true;
            mistAudioSource.Play();
        }
    }

    private void StopMistSound()
    {
        mistAudioSource.Stop();
    }

    private void ReduceMusicVolume()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = musicVolumeInMist;
        }
    }

    private void RestoreMusicVolume()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = originalMusicVolume;
        }
    }
}