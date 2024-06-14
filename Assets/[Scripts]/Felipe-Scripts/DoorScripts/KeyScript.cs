using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;
    public GameObject UIindicator;
    public Vector2 checkpointPosition;
    public int conditionId = 6;
    public bool startInactive = true;
    public SoundLibrary soundLibrary;

    public OBJECT_TYPE particleType;
    private GameObject particlePrefab;

    public float soundVolume = 1.0f;

    private void Start()
    {
        if (!GameManager.GetInstance().IsConditionCompleted(conditionId) && startInactive)
        {
            gameObject.SetActive(false);
        }
        else if (startInactive)
        {
            gameObject.SetActive(false);
            UIindicator.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            UIindicator.SetActive(false);
            SpawnParticleEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
                    AudioSource.PlayClipAtPoint(keySound, transform.position, soundVolume);
                }
            }

            Destroy(gameObject);
        }
    }

    private void SpawnParticleEffect()
    {
        particlePrefab = PoolManager.GetInstance().GetPooledObject(particleType, transform.position, Vector3.zero);
        if (particlePrefab == null)
        {
            Debug.LogError("No se pudo obtener el objeto de partículas del PoolManager.");
            return;
        }

        particlePrefab.SetActive(true);
    }
}