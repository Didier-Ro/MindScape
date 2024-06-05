using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;
    public GameObject UIindicator;
    public Vector2 checkpointPosition;
    public int conditionId = 6;
    public SoundLibrary soundLibrary;
    public GameObject particlePrefab;

    private void Start()
    {
        if (!GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            UIindicator.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("funciona");
            GameManager.GetInstance().MarkConditionCompleted(conditionId);
            CheckpointManager.AddCheckpointPosition(checkpointPosition);
            GameManager.GetInstance().SavePlayerPosition(checkpointPosition); 
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
                    Debug.LogError("No se encontró el script de DoorScript en el GameObject contenedor.");
                }
            }
            else
            {
                Debug.LogError("No se asignó ningún GameObject contenedor en el Inspector.");
            }

            if (soundLibrary != null)
            {
                AudioClip keySound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.KEYS);
                if (keySound != null)
                {
                    AudioSource.PlayClipAtPoint(keySound, transform.position);
                }
            }
            if (particlePrefab != null)
            {
                GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Destroy(particle, 2f);
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
    }
}