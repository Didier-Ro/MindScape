using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;
    public GameObject UIindicator;
    public Vector2 checkpointPosition;
    public int conditionId = 6;

    private void Start()
    {
        if (!GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           gameObject.SetActive(false);
        }
        else
        {
            UIindicator.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.AddCheckpointPosition(checkpointPosition);
            GameManager.GetInstance().SavePlayerPosition(checkpointPosition); 
            GameManager.GetInstance().MarkConditionCompleted(conditionId);
            GameManager.GetInstance().SaveAllData();
            UIindicator.SetActive(true);
            if (doorContainer != null)
            {
                DoorScript doorScript = doorContainer.GetComponent<DoorScript>();
                if (doorScript != null)
                {
                   
                }
                else
                {
                    Debug.LogError("No se encontr� el script de DoorScript en el GameObject contenedor.");
                }
            }
            else
            {
                Debug.LogError("No se asign� ning�n GameObject contenedor en el Inspector.");
            }
        }

        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        
    }
}