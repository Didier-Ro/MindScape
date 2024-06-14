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
    private AudioSource audioSource;

    private void Start()
    {
        playerRef = GameObject.Find("Player");
        if (playerRef != null)
        {
            flashLight = playerRef.GetComponent<Flashlight>();
            healthController = playerRef.GetComponent<HealthController>();
        }

        // Inicializa el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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

        if (playerInZone && !audioSource.isPlaying)
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
            StopMistSound();
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
            audioSource.clip = mistSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopMistSound()
    {
        audioSource.Stop();
    }
}