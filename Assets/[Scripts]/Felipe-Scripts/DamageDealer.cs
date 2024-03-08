using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private bool isPlayerInContact = false;
    public float damageDuration = 2f;

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
    IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(damageDuration);

        if (isPlayerInContact)
        {
            playerController.RespawnAtLastCheckpoint();
        }
    }
}