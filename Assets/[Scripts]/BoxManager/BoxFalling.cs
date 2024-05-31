 using System;
using UnityEngine;

public class BoxFalling : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private BoxCollider2D boxColliderParent;
    [SerializeField] private BoxCollider2D boxColliderChild;
    [SerializeField] private Vector3 spawnPoint;
    public Vector3 finalPoint;
    [Tooltip("If the box is SINGLE, then there is no need to reference DetectorManager.")]
    [SerializeField] private TYPE_BOX type_Box;
    [SerializeField] private OverlapBoxDetectorManager overlayBoxDetectorManager;
    [Tooltip("No poner nada, solo si necesitas el tutorial")]
    public GameObject tutorial;
    

    public BOX_STATE boxState = BOX_STATE.IDLE;
    public Action<BOX_STATE> OnBoxStateChange;

    private bool canMove = false;
    [SerializeField] private int conditionId;
    [SerializeField] private Vector3 positionWhenPuzzleIsCompleted;

    [SerializeField] bool isFalling;

    private float size;
    private float totalSize;

    void Start()
    {
        audioSource = GameManager.GetInstance().audioSource;

        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            boxColliderParent.enabled = false;
            transform.position = positionWhenPuzzleIsCompleted;
        }
        size = 1 - 0;
        totalSize = size / (60 * 1);
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            Falling();
        }

        if (canMove)
        {
            MoveBox();
        }
    }

    void Falling()
    {
        ChangeBoxState(BOX_STATE.FALLING);
        boxColliderParent.enabled = false;
        boxColliderChild.enabled = false;
        box.transform.localScale -= new Vector3(totalSize, totalSize, 0);

        if (box.transform.localScale.y <= 0 || box.transform.localScale.x <= 0)
        {
            isFalling = false;
            box.transform.localScale = new Vector3(0, 0, 0);

            if (type_Box == TYPE_BOX.DUO)
            {
                RespawnBox(overlayBoxDetectorManager.FindVoidPlace());
            }
            else if (type_Box == TYPE_BOX.SINGLE)
            {
                RespawnBox(finalPoint);
            }
        }
    }

    public void BoxInZone()
    {
        isFalling = true;
    }

    public void SetSpawnPosition(Vector3 _finalPoint)
    {
        finalPoint = _finalPoint;
        BoxInZone();
    }

    public void ChangeBoxState(BOX_STATE _newBoxState)
    {
        boxState = _newBoxState;
        if (OnBoxStateChange != null)
        {
            OnBoxStateChange.Invoke(boxState);
        }
    }

    public BOX_STATE GetBoxState()
    {
        return boxState;
    }

    void RespawnBox(Vector3 _finalPoint)
    {
        transform.position = _finalPoint;
        box.transform.position = new Vector3(_finalPoint.x, 27, 0);
        box.transform.localScale = new Vector3(1, 1, 1);
        ChangeBoxState(BOX_STATE.SPAWNING);
        spawnPoint = new Vector3(_finalPoint.x, 27, 0);
        finalPoint = _finalPoint;
        canMove = true;
    }

    void MoveBox()
    {
        float distance = spawnPoint.y - finalPoint.y;
        float destiny = distance / (60 * 1f);

        ChangeBoxState(BOX_STATE.MOVING);
        box.transform.position -= new Vector3(0, destiny, 0);

        if (box.transform.position.y <= finalPoint.y)
        {
            ChangeBoxState(BOX_STATE.IDLE);
            GameObject particle = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.ParticulaCajaCaida2, box.transform.position, Vector3.zero);
            if (particle != null)
            {
                particle.SetActive(true);
            }

            if (tutorial != null)
            {
                tutorial.SetActive(true);
            }
            PlayRandomImpactSound();
            box.transform.position = finalPoint;
            canMove = false;
            boxColliderParent.enabled = true;
            boxColliderChild.enabled = true;
        }
    }
    [SerializeField] SoundLibrary soundLibrary;
    [SerializeField] AudioSource audioSource;
    void PlayRandomImpactSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);
        AudioClip impactSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ROT_CAJAS_IMPACTO);
        if (impactSound != null)
        {
            // Play the sound
            audioSource.clip = impactSound;
            audioSource.Play();
        }
    }
}

public enum TYPE_BOX
{
    SINGLE,
    DUO,
}

public enum BOX_STATE
{
    IDLE,
    FALLING,
    SPAWNING,
    MOVING
}