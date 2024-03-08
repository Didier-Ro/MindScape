using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointManager.SetLastCheckpointPosition(checkpointPosition);
        }
    }
}