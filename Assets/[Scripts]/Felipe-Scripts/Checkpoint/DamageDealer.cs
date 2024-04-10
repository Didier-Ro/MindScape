using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private bool isPlayerInContact = false;
    private float contactTime = 0f;
    public float damageDuration = 5f;

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
            contactTime = 0f; // Reiniciar el tiempo cuando el jugador sale del contacto
        }
    }

    IEnumerator DamagePlayer()
    {
        while (isPlayerInContact && contactTime < damageDuration)
        {
            yield return new WaitForSeconds(1f); // Esperar un segundo
            contactTime += 1f; // Incrementar el tiempo de contacto
        }

        if (isPlayerInContact && contactTime >= damageDuration)
        {
            MovePlayerToNearestCheckpoint();
        }
    }

    private void MovePlayerToNearestCheckpoint()
    {
        Vector3 closestCheckpoint = CheckpointManager.FindNearestCheckpoint(transform.position);
        if (closestCheckpoint != Vector3.zero)
        {
            playerController.RespawnAtCheckpoint(closestCheckpoint);
        }
        else
        {
            Debug.LogWarning("No checkpoints found!");
        }
    }
}