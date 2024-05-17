using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Player health")]
    public float currentPlayerHealth = 100.0f;
    public float maxPlayerHealth = 100.0f;
    public GameObject gameOverScreen;
    public int regenRate = 1;
    private bool canRegen = false;

    [Header("Images")]
    public Image migraineImage = null;
    public Image migraineFlashImage = null;
    public float hurtTimer = 0.1f;

    [Header("Heal Timer")]
    public float healCooldown = 3.0f;
    public float maxHealCoolDown = 3.0f;
    private bool startCoolDown = false;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private int maxParticles = 5;

    [Header("Stress Particles")]
    public float stressThreshold = 0.5f; // Threshold for health percentage to start showing stress particles
    public Transform particleSpawnPoint; // Point to spawn particles
    private bool particlesSpawned = false;

    [Header("Audio")]
    public AudioSource audioSource; // Referencia al componente de audio
    public SoundLibrary soundLibrary; // Referencia al SoundLibrary
    public SOUND_TYPE whisperSoundType;

    void UpdatePlayerHealth()
    {
        Color imageAlpha = migraineImage.color;
        imageAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
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
            PlayerCameraShake.Instance.ShakeCamera(2f, 0.1f);
            CheckStressParticles();
        }
        else if (currentPlayerHealth <= 0)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.DEAD);
            currentPlayerHealth = 0;
            // gameOverScreen.SetActive(true);
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
            PlayWhisperSound(); // Reproduce el sonido cuando se activan las part�culas
        }
        else if (currentPlayerHealth >= maxPlayerHealth && particlesSpawned)
        {
            particlesSpawned = false;
            RemoveStressParticles();
            StopWhisperSound(); // Detiene el sonido cuando se desactivan las part�culas
        }
    }
    private void PlayWhisperSound()
    {
        if (audioSource != null && !audioSource.isPlaying && soundLibrary != null)
        {
            AudioClip whisperSound = soundLibrary.GetRandomSoundFromType(whisperSoundType);
            if (whisperSound != null)
            {
                audioSource.clip = whisperSound;
                audioSource.Play();
            }
        }
    }

    private void StopWhisperSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    [SerializeField] private float headHeight = 1.4f; // Ajusta la altura relativa de la cabeza del jugador

    void SpawnStressParticles()
    {
        // Verifica si el punto de generaci�n de part�culas est� asignado
        if (particleSpawnPoint == null)
        {
            Debug.LogError("Particle spawn point not assigned in the Inspector!");
            return;
        }

        Debug.Log("Spawning stress particles...");

        // Obtiene la posici�n de la cabeza del jugador restando un peque�o valor a la altura relativa de la cabeza
        Vector3 headPosition = transform.position + Vector3.up * (headHeight - 1f); // Resta 0.1 unidades para bajar un poco

        // Genera las part�culas
        GameObject estres1 = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Estres1, headPosition, Vector3.zero);
        GameObject estres2 = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Estres2Variant, headPosition, Vector3.zero);

        // Configura las part�culas generadas
        if (estres1 != null)
        {
            estres1.transform.SetParent(transform); // Establece la part�cula como hijo del jugador para seguimiento
            var particleSystem1 = estres1.GetComponent<ParticleSystem>();
            if (particleSystem1 != null)
            {
                particleSystem1.Play(); // Activa y reproduce el sistema de part�culas
            }
            else
            {
                Debug.LogWarning("Estres1 particle system component not found.");
            }
        }

        if (estres2 != null)
        {
            estres2.transform.SetParent(transform); // Establece la part�cula como hijo del jugador para seguimiento
            var particleSystem2 = estres2.GetComponent<ParticleSystem>();
            if (particleSystem2 != null)
            {
                particleSystem2.Play(); // Activa y reproduce el sistema de part�culas
            }
            else
            {
                Debug.LogWarning("Estres2Variant particle system component not found.");
            }
        }
    }

    void RemoveStressParticles()
    {
        Debug.Log("Removing stress particles...");
        // Deactivate and stop all child particles
        foreach (Transform child in transform)
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
    }
}