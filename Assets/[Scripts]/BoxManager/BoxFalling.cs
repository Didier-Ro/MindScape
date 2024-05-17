using System;
using UnityEngine;

public class BoxFalling : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private BoxCollider2D boxColliderParent;
    [SerializeField] private BoxCollider2D boxColliderChild;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 finalPoint;
    [Tooltip("If the box is SINGLE, then there is no need to reference DetectorManager.")]
    [SerializeField] private TYPE_BOX type_Box;
    [SerializeField] private OverlapBoxDetectorManager overlayBoxDetectorManager;
    [SerializeField] private PoolManager poolManager; // Reference to the PoolManager
    [SerializeField] private Transform groundCheck; // Transform that checks for ground collision
    [SerializeField] private LayerMask groundLayer; // Layer mask for the ground

    public BOX_STATE boxState = BOX_STATE.IDLE;
    public Action<BOX_STATE> OnBoxStateChange;

    private bool canMove = false;
    [SerializeField] private int conditionId;
    [SerializeField] private Vector3 positionWhenPuzzleIsCompleted;
    [SerializeField] private bool isFalling;

    private float size;
    private float totalSize;
    private ActivateZone activateZone;
    [SerializeField] private SoundLibrary soundLibrary;


    void Start()
    {
        activateZone = GetComponent<ActivateZone>();

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
            activateZone.DeactivateCanvas();
            Falling();
        }

        if (canMove)
        {
            MoveBox();
        }
    }

    public void BoxInZone()
    {
        isFalling = true;
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

            // Reproducir sonido de caja impacto
            PlayRandomImpactSound();
        }
    }
    void PlayRandomImpactSound()
    {
        // Asegúrate de que el SoundLibrary esté asignado
        if (soundLibrary == null)
        {
            Debug.LogError("SoundLibrary is not assigned!");
            return;
        }

        // Obtener un sonido aleatorio de caja impacto de la SoundLibrary
        AudioClip impactSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.ROT_CAJAS_IMPACTO);

        // Reproducir el sonido si se encontró
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }
        else
        {
            Debug.LogWarning("Impact sound not found!");
        }
    }
    public void SetSpawnPosition(Vector3 _finalPoint)
    {
        finalPoint = _finalPoint;
        BoxInZone();
    }

    public void ChangeBoxState(BOX_STATE _newBoxState)
    {
        boxState = _newBoxState;
        OnBoxStateChange?.Invoke(boxState);
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
            box.transform.position = finalPoint;
            canMove = false;
            boxColliderParent.enabled = true;
            boxColliderChild.enabled = true;

            // Spawn falling particles when the box becomes idle
            SpawnFallingParticles();

            // Reproducir el sonido de impacto cuando la caja vuelve al estado IDLE después de caer
            PlayRandomImpactSound();
        }
    }

    void SpawnFallingParticles()
    {
        // Get a falling particle object from the pool
        GameObject particle = poolManager.GetPooledObject(OBJECT_TYPE.ParticulaCajaCaída2, transform.position, Vector3.zero);

        // If a particle object was successfully retrieved from the pool
        if (particle != null)
        {
            // Activate the particle
            particle.SetActive(true);
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