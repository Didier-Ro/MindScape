using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorContainer != null)
            {
                DoorScript doorScript = doorContainer.GetComponent<DoorScript>();
                if (doorScript != null)
                {
                    doorScript.isUnlocked = true;
                }
                else
                {
                    Debug.LogError("No se encontró el script de DoorScript en el GameObject contenedor.");
                }
            }
            else
            {
                Debug.LogError("No se asignó ningún GameObject contenedor en el Inspector.");
            }
        }

        Destroy(gameObject);
    }
}