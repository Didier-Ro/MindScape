using System;
using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private BoxCollider2D colliderParent;
    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private TYPE_DETECTOR typeDetector;
    [SerializeField] private Animator animator;

    [SerializeField] private Doors doors;
    [SerializeField] private int conditionId;
    [SerializeField] private Transform transformPoint;

    [SerializeField] private GameObject boxJail;
    [SerializeField] private Collider2D collider1;
    [SerializeField] private Collider2D collider2;
    [SerializeField] private BoxFalling boxFalling;
    [Tooltip("No poner nada, solo si necesitas el tutorial")]
    public GameObject tutorial;

    private GameObject clonPrefab;
    private Transform player;
    private Transform playerSprite;

    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            animator.SetBool("Pressed", true);
            if (typeDetector == TYPE_DETECTOR.HOLE)
            {
                doors.IncreaseHoleCounter();
                if (doors.ReturnHoleCounter() <= doors.holeNumbers)
                {
                    GameObject particle = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.ParticulaPoof, boxJail.transform.position, Vector3.zero);
                    if (particle != null)
                    {
                        // Activar y reproducir la part�cula
                        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Play();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No se pudo obtener la part�cula ParticulaPoof del PoolManager.");
                    }

                    boxJail.SetActive(false);
                    collider1.enabled = true;
                    collider2.enabled = true;
                    boxFalling.finalPoint = spawnPos;
                }
                gameObject.SetActive(false);
            }
            else if (typeDetector == TYPE_DETECTOR.BUTTON)
            {
                doors.IncreaseCounter();
                if (doors.ReturnCounter() != doors.buttonNumbers || doors.ReturnHoleCounter() != doors.holeNumbers)
                {
                    clonPrefab = Instantiate(boxPrefab, new Vector3(spawnPos.x, 27, 0), Quaternion.identity);
                    if (tutorial != null)
                    {
                        clonPrefab.GetComponent<BoxFalling>().tutorial = tutorial;
                    }
                    clonPrefab.GetComponent<BoxFalling>().SetSpawnPosition(spawnPos);
                    CameraManager.instance.ChangeTargetCamera(clonPrefab);
                }
            }
            else if (typeDetector == TYPE_DETECTOR.UNIQUE)
            {
                doors.IncreaseCounter();
            }
            Transform parentTransform = collision.transform.parent;
            GameObject parent = parentTransform.gameObject;
            colliderParent = parent.GetComponent<BoxCollider2D>();
            colliderParent.enabled = false;
        }

        if (collision.CompareTag("Feet"))
        {
            AudioManager.GetInstance().SetSound(SOUND_TYPE.ROT_PLACA_DE_PRESION);
            player = PlayerStates.GetInstance().transform;
            playerSprite = player.transform.Find("Sprite");
            playerSprite.localPosition = new Vector3(0f, -0.15f, 0f);
            animator.SetBool("Pressed", true);
            if (typeDetector == TYPE_DETECTOR.HOLE)
            {

            }
            else if (typeDetector == TYPE_DETECTOR.BUTTON)
            {
                doors.IncreaseCounter();
            }
            else if (typeDetector == TYPE_DETECTOR.UNIQUE)
            {
                doors.IncreaseCounter();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (typeDetector == TYPE_DETECTOR.BUTTON || typeDetector == TYPE_DETECTOR.UNIQUE)
        {
            animator.SetBool("Pressed", false);
            playerSprite = player.Find("Sprite");
            playerSprite.localPosition = new Vector3(0f, -0.1f, 0f);
            if (collision.CompareTag("Feet"))
            {
                doors.DecreaseCounter();
            }
        }
    }
}

public enum TYPE_DETECTOR
{
    HOLE,
    BUTTON,
    UNIQUE
}