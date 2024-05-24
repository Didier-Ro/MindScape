using UnityEngine;

public class GoalAnimEvent : MonoBehaviour
{
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private SoundLibrary soundLibrary;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
        doorToUnlock.SetActive(false);
        gameObject.SetActive(false);
        PlayObjectBreakSound();
    }

    private void PlayOrbDestructionSound()
    {
        AudioClip orbSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ORBE_DE_CRISTAL);
        if (orbSound != null)
        {
            audioSource.PlayOneShot(orbSound);
        }
    }

    private void PlayObjectBreakSound()
    {
        AudioClip breakSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ORBE_DE_CRISTAL_ROTO);
        if (breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }
    }
}