using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private bool boxOnButton = false;
    [SerializeField] private bool doorIsOpen = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boxOnButton = true;
            if (!doorIsOpen)
            {
                OpenDoor();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boxOnButton = false;
        }
    }
    private void OpenDoor()
    {
        Door.SetActive(false); // Desactiva la puerta
        boxOnButton = true;
    }
}
