using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private bool boxOnButton = false;
    [SerializeField] private bool doorIsOpen = false;
    private byte buttonCounter = 0;
    [SerializeField] private int conditionId;


    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           Destroy(gameObject);
        }
    }

    private void OpenDoor()
    {
        StartCoroutine(CameraManager.instance.ChangeCameraToThePlayer(1));
        Door.SetActive(false); // Desactiva la puerta
    }

    private void CloseDoor()
    {
        Door.SetActive(true);
    }

    public void IncreaseCounter()
    {
        buttonCounter++;

        if (buttonCounter == 2)
        {
            CameraManager.instance.ChangeCameraToAnObject(gameObject);
            OpenDoor();
        }
    }

    public void DecreaseCounter()
    {
        buttonCounter--;
        if (buttonCounter < 2)
        {
            CloseDoor();
        }
    }

    public byte ReturnCounter()
    {
        return buttonCounter;
    }
}
