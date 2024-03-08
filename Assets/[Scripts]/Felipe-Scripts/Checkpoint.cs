using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // La posici�n del checkpoint en el mundo
    public Vector3 checkpointPosition;

    // Este m�todo se llama cuando un objeto entra en el �rea del collider del checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que entra es el jugador
        if (collision.CompareTag("Player"))
        {
            // Establece la posici�n del �ltimo checkpoint alcanzado en esta posici�n
            CheckpointManager.SetLastCheckpointPosition(checkpointPosition);
        }
    }
}