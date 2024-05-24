using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public GameObject doorContainer;
    public GameObject UIindicator;
    public int conditionId = 6;
    public SoundLibrary soundLibrary; // Referencia al scriptable object de la biblioteca de sonidos
    public GameObject particlePrefab; // Prefab de la partícula de la llave

    private void Start()
    {
        if (!GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject);
            if (doorContainer != null)
            {
                DoorScript doorScript = doorContainer.GetComponent<DoorScript>();
                if (doorScript != null)
                {
                    UIindicator.SetActive(true);
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

            // Reproducir sonido de la llave si la biblioteca de sonidos y el tipo de sonido están configurados
            if (soundLibrary != null)
            {
                AudioClip keySound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.KEYS);
                if (keySound != null)
                {
                    AudioSource.PlayClipAtPoint(keySound, transform.position);
                }
            }

            // Activar la partícula de la llave si el prefab está configurado
            if (particlePrefab != null)
            {
                GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                Destroy(particle, 2f); // Destruir la partícula después de 2 segundos
            }
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance().MarkConditionCompleted(conditionId);
        UIindicator.SetActive(true);
    }
}