using UnityEngine;

public static class CheckpointManager
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