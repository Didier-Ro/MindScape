using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthController : MonoBehaviour
{
    [Header("Player health")]
    public float currentPlayerHealth = 100.0f;
    public float maxPlayerHealth = 100.0f;
    public GameObject gameOverScreen;
    public int regenRate = 1;
    private bool canRegen = false;
    [SerializeField] private Volume postProcessingVolume;
    private ChromaticAberration chromaticAberration;

    [Header("Images")]
    public Image migraineImage = null;
    public Image migraineFlashImage = null;
    public float hurtTimer = 0.1f;

    [Header("Heal Timer")]
    public float healCooldown = 3.0f;
    public float maxHealCoolDown = 3.0f;
    private bool startCoolDown = false;

    [Header("Stress Particles")]
    public float stressThreshold = 0.5f;
    public Transform particleSpawnPoint;
    private bool particlesSpawned = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public SoundLibrary soundLibrary;
    public AudioMixer masterMixer;
    private const float minHealthForSuffering = 100f;
    private bool hasPlayedSufferingSound = false;


    private void Start()
    {
        postProcessingVolume.profile.TryGet(out chromaticAberration);
    }

    void UpdatePlayerHealth()
    {
        Color imageAlpha = migraineImage.color;
        imageAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        chromaticAberration.intensity.value = 1 - (currentPlayerHealth / maxPlayerHealth);
        migraineImage.color = imageAlpha;
    }

    public void PlayerTakeDamage(float damage)
    {
        currentPlayerHealth -= damage;

        if (currentPlayerHealth > 0)
        {
            canRegen = false;
            UpdatePlayerHealth();
            healCooldown = maxHealCoolDown;
            startCoolDown = true;
            PlayerCameraShake.Instance.ShakeCamera(1f, 0.1f);
            CheckStressParticles();

            hasPlayedSufferingSound = false;
        }
        else if (currentPlayerHealth <= 0)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.DEAD);
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DEAD);
            currentPlayerHealth = 0;
            Debug.Log("Player is dead");
        }
    }

    void Regen()
    {
        if (startCoolDown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCoolDown = false;
            }
        }

        if (canRegen)
        {
            if (currentPlayerHealth <= maxPlayerHealth - 0.01f)
            {
                currentPlayerHealth += Time.deltaTime * regenRate;
                UpdatePlayerHealth();
                CheckStressParticles();
            }
            else
            {
                currentPlayerHealth = maxPlayerHealth;
                healCooldown = maxHealCoolDown;
                canRegen = false;
                RemoveStressParticles();
            }
        }
    }

    void CheckStressParticles()
    {
        if (currentPlayerHealth < maxPlayerHealth && !particlesSpawned)
        {
            SpawnStressParticles();
            particlesSpawned = true;
            PlayWhisperSound();
        }
        else if (currentPlayerHealth >= maxPlayerHealth && particlesSpawned)
        {
            particlesSpawned = false;
            RemoveStressParticles();
            StopWhisperSound();
        }
    }


    private void PlayRandomSufferingSound()
    {
        if (audioSource != null && soundLibrary != null)
        {
            AudioClip sufferingClip = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.Sufrimiento);

            if (sufferingClip != null)
            {
                audioSource.clip = sufferingClip;
                audioSource.Play();

                hasPlayedSufferingSound = true;
            }
            else
            {
                Debug.LogWarning("No se encontr� ning�n sonido de sufrimiento en la SoundLibrary.");
            }
        }
    }

    private void PlayWhisperSound()
    {
        if (audioSource != null && soundLibrary != null)
        {
            AudioClip whisperSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ROT_SUSURROS_ENEMIGO);
            if (whisperSound != null)
            {
                audioSource.clip = whisperSound;
                audioSource.loop = true;
                audioSource.Play();
                masterMixer.SetFloat("WhisperVolume", 0.0f);
            }
            else
            {
                Debug.LogWarning("Whisper sound not found in the SoundLibrary.");
            }
        }
    }

    private void StopWhisperSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.loop = false;
            audioSource.Stop();
            masterMixer.SetFloat("WhisperVolume", -80.0f);
        }
    }

    void SpawnStressParticles()
    {
        if (particleSpawnPoint == null)
        {
            Debug.LogError("Particle spawn point not assigned in the Inspector!");
            return;
        }

        Vector3 spawnPosition = particleSpawnPoint.position;

        GameObject estres1 = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Estres1, spawnPosition, Quaternion.identity.eulerAngles);
        GameObject estres2 = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Estres2Variant, spawnPosition, Quaternion.identity.eulerAngles);

        if (estres1 != null)
        {
            estres1.transform.SetParent(transform);
            estres1.transform.localPosition = Vector3.zero;
            var particleSystem1 = estres1.GetComponent<ParticleSystem>();
            if (particleSystem1 != null)
            {
                particleSystem1.Play();
            }
            else
            {
                Debug.LogWarning("Estres1 particle system component not found.");
            }
        }

        if (estres2 != null)
        {
            estres2.transform.SetParent(transform);
            estres2.transform.localPosition = Vector3.zero;
            var particleSystem2 = estres2.GetComponent<ParticleSystem>();
            if (particleSystem2 != null)
            {
                particleSystem2.Play();
            }
            else
            {
                Debug.LogWarning("Estres2Variant particle system component not found.");
            }
        }
    }

    void RemoveStressParticles()
    {
        // Debug.Log("Removing stress particles...");
        foreach (Transform child in particleSpawnPoint)
        {
            var particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        Regen();

        if (currentPlayerHealth < minHealthForSuffering && !hasPlayedSufferingSound && !audioSource.isPlaying)
        {
            PlayRandomSufferingSound();
        }
    }
}