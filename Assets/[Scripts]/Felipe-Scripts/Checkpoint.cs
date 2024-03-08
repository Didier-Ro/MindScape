using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // La posición del checkpoint en el mundo
    public Vector3 checkpointPosition;

    // Este método se llama cuando un objeto entra en el área del collider del checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que entra es el jugador
        if (collision.CompareTag("Player"))
        {
            // Establece la posición del último checkpoint alcanzado en esta posición
            CheckpointManager.SetLastCheckpointPosition(checkpointPosition);
        }
    }
}