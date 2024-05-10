using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAnimEvent : MonoBehaviour
{
    [SerializeField] private GameObject doorToUnlock;
    public void StartAnimEvent()
    {
        Debug.Log("START");
        Debug.Log(gameObject.transform.position);
        CameraManager.instance.ChangeCameraToAnObject(gameObject);
    }

    public void EndAnimEvent()
    {
        Debug.Log("END");
        CameraManager.instance.ChangeCameraToThePlayer();
        doorToUnlock.SetActive(false);
        gameObject.SetActive(false);
    }
}
