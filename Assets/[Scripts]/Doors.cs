using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private bool boxOnButton = false;
    [SerializeField] private bool doorIsOpen = false;
    [SerializeField] private int conditionId;
    public int holeNumbers;
    [SerializeField] private int buttonNumbers;
    private byte buttonCounter = 0;
    private byte holeCounter = 0;


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

        if (buttonCounter == buttonNumbers)
        {
            CameraManager.instance.ChangeCameraToAnObject(gameObject);
            OpenDoor();
        }
    }

    public void IncreaseHoleCounter()
    {
        holeCounter++;
    }

    public byte ReturnHoleCounter()
    {
        return holeCounter;
    }

    public void DecreaseCounter()
    {
        buttonCounter--;
        if (buttonCounter < buttonNumbers)
        {
            CloseDoor();
        }
    }

    public byte ReturnCounter()
    {
        return buttonCounter;
    }
}
