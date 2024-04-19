using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 checkpointPosition;
    [SerializeField] private int condition;
    [SerializeField] private bool isSavedInGame = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointManager.AddCheckpointPosition(checkpointPosition);
            if (isSavedInGame)
            {
                GameManager.GetInstance().SavePlayerPosition(checkpointPosition);
                GameManager.GetInstance().MarkConditionCompleted(condition);
            }
        }
    }
}