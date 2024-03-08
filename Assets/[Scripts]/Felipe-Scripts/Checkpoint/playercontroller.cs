using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private static Transform playerTransform;
    public List<string> inventory = new List<string>();

    private void Awake()
    {
        playerTransform = transform;
    }

    public static void DisappearPlayer()
    {
        playerTransform.gameObject.SetActive(false);
    }

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