using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private static Transform playerTransform;

    private void Awake()
    {
        playerTransform = transform;
    }

    // Método estático para respawnear al jugador en el último checkpoint alcanzado
    public static void RespawnAtLastCheckpoint()
    {
        if (CheckpointManager.GetLastCheckpointPosition() != Vector3.zero)
        {
            playerTransform.position = CheckpointManager.GetLastCheckpointPosition();
        }
        else
        {
            Debug.LogWarning("No checkpoint reached yet!");
        }
    }
}