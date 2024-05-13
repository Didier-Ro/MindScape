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
    [SerializeField] private byte buttonCounter = 0;
    private byte holeCounter = 0;
    private float delayStart = 3;


    private void Start()
    {
        delayStart *= 60;
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           Destroy(gameObject);
        }
    }
    
    private IEnumerator CoroutineToReturnCamera(bool isOpen)
    {
        
        for (int i = 0; i < delayStart; i++)
        {
            yield return null; 
        }
        CameraManager.instance.ChangeCameraToThePlayer();
        if (isOpen)
        {
         Door.SetActive(false);// Desactiva la puerta
        }
        else
        {
            Door.SetActive(true);
        }
    }

    private void OpenDoor()
    {
        StartCoroutine(CoroutineToReturnCamera(true));
    }

    private void CloseDoor()
    {
        StartCoroutine(CoroutineToReturnCamera(false));
    }

    public void IncreaseCounter()
    {
        buttonCounter++;
        if (buttonCounter >= buttonNumbers)
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
