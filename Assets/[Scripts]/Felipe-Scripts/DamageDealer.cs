using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private bool isPlayerInContact = false;
    public float damageDuration = 2f; // Tiempo en segundos que el jugador debe estar en contacto con el enemigo para recibir daño

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInContact = true;
            StartCoroutine(DamagePlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInContact = false;
        }
    }

    // Corutina para activar el daño al jugador después de un tiempo especificado
    IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(damageDuration);

        if (isPlayerInContact)
        {
            PlayerController.RespawnAtLastCheckpoint();
        }
    }
}