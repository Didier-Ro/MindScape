using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointPosition;
    [SerializeField] private int condition;
    [SerializeField] private bool isSavedInGame = false;
    [SerializeField] private PlayerRespawnPositon playerRespawnPositon;

    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(condition))
        {
             Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRespawnPositon.SetCheckPointSpawnPosition(checkpointPosition);
            if (collision.TryGetComponent(out Falling fallingScript))
            {
                fallingScript.SetPlayerRespawnPosition();
            }
            CheckpointManager.AddCheckpointPosition(checkpointPosition);
            if (isSavedInGame)
            {
                GameManager.GetInstance().SavePlayerPosition(checkpointPosition);
                Debug.Log("entra");
                GameManager.GetInstance().MarkConditionCompleted(condition);
                GameManager.GetInstance().SaveAllData();
            }
        }
    }
}