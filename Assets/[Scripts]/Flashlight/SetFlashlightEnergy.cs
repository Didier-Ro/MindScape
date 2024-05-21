using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlashlightEnergy : MonoBehaviour
{
    [SerializeField] private FlashlightEnergy flashlightEnergy;
    [SerializeField] private Flashlight flashlight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}
