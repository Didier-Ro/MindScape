using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Marcar este checkpoint como activo
            GameManagerCheckpoints.instance.SetCheckpoint(transform.position);

            // Desactivar este collider para evitar múltiples activaciones
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
