using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAnimEvent : MonoBehaviour
{
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private SoundLibrary soundLibrary;
    public void StartAnimEvent()
    {
        Debug.Log("START");
        Debug.Log(gameObject.transform.position);
        CameraManager.instance.ChangeCameraToAnObject(gameObject);
        AudioClip soundClip = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ORBE_DE_CRISTAL);
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

    }
}
