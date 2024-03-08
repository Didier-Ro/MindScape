using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static Vector3 lastCheckpointPosition;

    // Método estático para establecer la posición del último checkpoint alcanzado
    public static void SetLastCheckpointPosition(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    // Método estático para obtener la posición del último checkpoint alcanzado
    public static Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}