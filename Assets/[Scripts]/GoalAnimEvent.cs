using UnityEngine;

public class GoalAnimEvent : MonoBehaviour
{
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private SoundLibrary soundLibrary;
    public AudioSource audioSource;

    private AudioSource playerAudioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
            if (playerAudioSource == null)
            {
                playerAudioSource = player.AddComponent<AudioSource>();
            }
        }
    }

    public void StartAnimEvent()
    {
        Debug.Log("START");
        Debug.Log(gameObject.transform.position);
        CameraManager.instance.ChangeCameraToAnObject(gameObject);
        PlayOrbDestructionSound();
    }

    public void LookAtDoor()
    {
        CameraManager.instance.ChangeCameraToAnObject(doorToUnlock);
    }

    public void EndAnimEvent()
    {
        Debug.Log("END");
        CameraManager.instance.ChangeCameraToThePlayer();

        // Detener el sonido de orbe de cristal
        audioSource.Stop();

        // Reproducir el sonido roto inmediatamente después de detener el sonido de orbe de cristal
        PlayObjectBreakSound();

        // Desactivar los objetos
        doorToUnlock.SetActive(false);
        gameObject.SetActive(false);
    }

    private void PlayOrbDestructionSound()
    {
        AudioClip orbSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ORBE_DE_CRISTAL);
        if (orbSound != null)
        {
            // Reproducir sonido en el audioSource
            audioSource.PlayOneShot(orbSound);
        }
    }

    private void PlayObjectBreakSound()
    {
        AudioClip breakSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ORBE_DE_CRISTAL_ROTO);
        if (breakSound != null && playerAudioSource != null)
        {
            // Reproducir el sonido roto inmediatamente
            playerAudioSource.PlayOneShot(breakSound);
        }
    }
}