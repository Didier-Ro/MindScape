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
            if (collision.TryGetComponent(out Falling fallingScript))
            {
                fallingScript.SetPlayerRespawnPosition(checkpointPosition);
            }
            CheckpointManager.AddCheckpointPosition(checkpointPosition);
            if (isSavedInGame)
            {
                GameManager.GetInstance().SavePlayerPosition(checkpointPosition);
                GameManager.GetInstance().MarkConditionCompleted(condition);
            }
            playerRespawnPositon.respawnPositionCheckPoint = checkpointPosition;
        }
    }
}