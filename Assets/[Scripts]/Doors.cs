using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private bool boxOnButton = false;
    [SerializeField] private bool doorIsOpen = false;
    private byte buttonCounter = 0;
    
   
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
