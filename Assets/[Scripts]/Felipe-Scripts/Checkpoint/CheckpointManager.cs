using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    private static Vector3 lastCheckpointPosition;
    
    public static void SetLastCheckpointPosition(Vector3 position)
    {
        checkpointPositions.Add(position);
    }

    // Encuentra el checkpoint m�s cercano a la posici�n dada
    public static Vector3 FindNearestCheckpoint(Vector3 currentPosition)
    {
        if (checkpointPositions.Count == 0)
        {
            return Vector3.zero; // No hay checkpoints disponibles
        }

        Vector3 nearestCheckpoint = checkpointPositions[0];
        float shortestDistance = Vector3.Distance(nearestCheckpoint, currentPosition);

        // Itera sobre todas las posiciones de los checkpoints y encuentra la m�s cercana
        foreach (Vector3 checkpointPosition in checkpointPositions)
        {
            float distanceToCheckpoint = Vector3.Distance(checkpointPosition, currentPosition);
            if (distanceToCheckpoint < shortestDistance)
            {
                nearestCheckpoint = checkpointPosition;
                shortestDistance = distanceToCheckpoint;
            }
        }

        return nearestCheckpoint;
    }
}