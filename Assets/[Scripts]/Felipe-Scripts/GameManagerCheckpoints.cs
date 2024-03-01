using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCheckpoints : MonoBehaviour
{
    public static GameManagerCheckpoints instance;

    private Vector3 checkpointPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    public Vector3 GetCheckpoint()
    {
        return checkpointPosition;
    }
}
