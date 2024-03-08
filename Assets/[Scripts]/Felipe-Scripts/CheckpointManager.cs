using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static Vector3 lastCheckpointPosition;

    // M�todo est�tico para establecer la posici�n del �ltimo checkpoint alcanzado
    public static void SetLastCheckpointPosition(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    // M�todo est�tico para obtener la posici�n del �ltimo checkpoint alcanzado
    public static Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}