using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFuel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Flashlight.GetInstance().isInInitialRoom = false;
        }
    }
}
