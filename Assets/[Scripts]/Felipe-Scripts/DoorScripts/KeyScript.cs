using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;
    public GameObject UIindicator;
    public int conditionId = 6;

    private void Start()
    {
        if (!GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject);
            if (doorContainer != null)
            {
                DoorScript doorScript = doorContainer.GetComponent<DoorScript>();
                if (doorScript != null)
                {
                    UIindicator.SetActive(true);
                }
                else
                {
                    Debug.LogError("No se encontr� el script de DoorScript en el GameObject contenedor.");
                }
            }
            else
            {
                Debug.LogError("No se asign� ning�n GameObject contenedor en el Inspector.");
            }
        }

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        UIindicator.SetActive(true);
    }
}