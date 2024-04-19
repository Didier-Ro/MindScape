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
        Door.SetActive(false); // Desactiva la puerta
    }

    public void IncreaseCounter()
    {
        buttonCounter++;

        if (buttonCounter == 2)
        {
            OpenDoor();
        }
    }

    public byte ReturnCounter()
    {
        return buttonCounter;
    }
}
