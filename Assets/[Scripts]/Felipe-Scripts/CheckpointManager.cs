using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static Vector3 lastCheckpointPosition;

    public static void SetLastCheckpointPosition(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public static Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}