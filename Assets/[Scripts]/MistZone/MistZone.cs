using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistZone : MonoBehaviour
{
    [SerializeField] private float damagePerSecond;
    [SerializeField]private GameObject playerRef;
    private Flashlight flashLight;
    private HealthController healthController;
    private bool playerInZone;
    private bool canDamage;

    private void Start()
    {
        playerRef = GameObject.Find("Player");
        if (playerRef != null)
        {
            flashLight = playerRef.GetComponent<Flashlight>();
            healthController = playerRef.GetComponent<HealthController>();
        }
    }

    private void Update()
    {
        if (playerInZone && flashLight.currentSliderValue <= 0)
        {
            canDamage = true;
        }
        else
        {
            canDamage = false;  
        }
    }

    private void FixedUpdate()
    {
        if (canDamage)
        {
            MakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    private void MakeDamage()
    {
        float totalDamage = damagePerSecond / 60;
        healthController.PlayerTakeDamage(totalDamage);
    }
}
