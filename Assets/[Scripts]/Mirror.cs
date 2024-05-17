using System;
using Unity.VisualScripting;
using UnityEngine;

public class Mirror : MonoBehaviour, Ikillable
{
    [SerializeField] private Transform hitPoint;
    [SerializeField] private Transform outPoint;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lightLenght = 10f;
    [SerializeField] private float angleRange = 160f;
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject lightGoal;
    [SerializeField] private Transform[] hitPositions;
    [SerializeField] private float secondsNeedToDisplayRay;
    [SerializeField] private float recoverTime;
    [SerializeField] private Renderer goalRenderer;
    [SerializeField] private Animator goalAnimator;
    [SerializeField] private MirrorPosition _mirrorPosition;
    private MirrorStates mirrorStates = MirrorStates.IDLE;
    //[SerializeField] private UnityEvent onAnimationEvent;
    private Renderer _renderer;
    private Vector3 directionToShotTheRaycast;
    private float framesHit;
    private float parentOffset;
    private float initialAngleRange;
    private float mediumAngleRange;
    private float upperAngleRange;
    private bool startPlayingParticles;


    private void Start()
    {
        _renderer = parentObject.GetComponent<Renderer>();
        GameManager.GetInstance().OnFlashingChange += TurnOffLight;
        ChangeHitPosition();
    }

    private void FixedUpdate()
    {
        switch (mirrorStates)
        {
            case MirrorStates.IDLE:
                break;
            case MirrorStates.REFLECTING:
                MirrorProjection();
                break;
            case MirrorStates.COOLING_TIME:
                HealObject();
                break;
            case MirrorStates.RECEIVING_LIGHT:
                break;
        }
        OverheatEffect();
    }

    private void OverheatEffect()
    {
        float normalizedValue = Mathf.Clamp01(framesHit / (secondsNeedToDisplayRay*60f));
        Color finalColor = Color.Lerp(Color.white, Color.red, normalizedValue);
        _renderer.material.color = finalColor;
    }

    private void TurnOffLight(bool isFlashing)
    {
        if (!isFlashing)
        {
            UnHit(transform);
        }
    }

    private void HealObject()
    {
        if (framesHit <= 0)
        {
            mirrorStates = MirrorStates.IDLE;
            recoverTime = 60;
            lineRenderer.enabled = false;
            lineRenderer.SetPosition(1, outPoint.position + outPoint.TransformDirection(Vector3.left) * lightLenght);
            startPlayingParticles = false;
            hitParticles.Stop(true);
        }
        else if(recoverTime <= 0)
        {
            MirrorProjection();
            framesHit--;
        }
        else
        {
            MirrorProjection();
            recoverTime--;
        }
    }
    
    private void ChangeHitPosition()
    {
        Vector2 positionsToSpawn = Vector2.zero;
        switch (_mirrorPosition)
        {
            case MirrorPosition.UP:
                positionsToSpawn.x = 0;
                positionsToSpawn.y = 2;
                directionToShotTheRaycast = Vector3.right;
                mediumAngleRange = 90;
                break;
            case MirrorPosition.UPL:
                positionsToSpawn.x = 0;
                positionsToSpawn.y = 3;
                directionToShotTheRaycast = Vector3.left;
                break;
            case MirrorPosition.DOWN:
                positionsToSpawn.x = 1;
                positionsToSpawn.y = 3;
                directionToShotTheRaycast = Vector3.left;
                mediumAngleRange = 270;
                break;
            case MirrorPosition.DOWNL:
                positionsToSpawn.x = 1;
                positionsToSpawn.y = 2;
                directionToShotTheRaycast = Vector3.right;
                mediumAngleRange = 270;
                break;
            case MirrorPosition.RIGHT:
                positionsToSpawn.x = 2;
                positionsToSpawn.y = 1;
                directionToShotTheRaycast = Vector3.down;
                mediumAngleRange = 0;
                break;
            case MirrorPosition.RIGHTL:
                positionsToSpawn.x = 2;
                positionsToSpawn.y = 0;
                directionToShotTheRaycast = Vector3.up;
                mediumAngleRange = 0;
                break;
            case MirrorPosition.LEFT:
                positionsToSpawn.x = 3;
                positionsToSpawn.y = 0;
                directionToShotTheRaycast = Vector3.up;
                mediumAngleRange = 180;
                break;
            case MirrorPosition.LEFTL:
                positionsToSpawn.x = 3;
                positionsToSpawn.y = 1;
                directionToShotTheRaycast = Vector3.down;
                mediumAngleRange = 180;
                break;
        }
       // outPoint.transform.rotation =  Quaternion.Euler(outPoint.transform.rotation.x, outPoint.transform.rotation.y, mediumAngleRange);
        hitPoint.position = hitPositions[(int)positionsToSpawn.x].position;
        outPoint.position = hitPositions[(int)positionsToSpawn.y].position;
        initialAngleRange = Mathf.Repeat(mediumAngleRange - angleRange/ 2f, 360);
        upperAngleRange = Mathf.Repeat(mediumAngleRange + angleRange / 2f, 360);
    }

    public void Hit(Transform player)
    {
        if (AngleCheck(player))
        { 
            if (framesHit > secondsNeedToDisplayRay * 60)
            {
                mirrorStates = MirrorStates.REFLECTING;
            }
            else
            {
                mirrorStates = MirrorStates.RECEIVING_LIGHT;
                recoverTime = 60;
                framesHit++;
            }
        }
    }

    private float ReduceErrorZero(float value)
    {
        if (Mathf.Approximately(value, 0))
        {
            value = Mathf.Epsilon;
        }
        return value;
    }
    private bool AngleCheck(Transform player)
    {
        bool canSeeTarget;
        Vector2 direction = player.position - parentObject.transform.position;
        direction.x = ReduceErrorZero(direction.x);
        direction.y = ReduceErrorZero(direction.y);
        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = Mathf.Repeat(angleRadians * Mathf.Rad2Deg, 360);
        Debug.Log(angleDegrees);
        if (initialAngleRange < upperAngleRange)
        {
            return canSeeTarget = upperAngleRange >= angleDegrees && angleDegrees >= initialAngleRange;
        }
        else
        {
            bool secondSegment = angleDegrees <= upperAngleRange && initialAngleRange >= 360 - angleRange;
            bool firstSegment = angleDegrees >= initialAngleRange && upperAngleRange <= angleRange;
             return  canSeeTarget = secondSegment || firstSegment;
        }
    }

    public void MirrorProjection()
    {
       // Vector3 direction = outPoint.TransformDirection(Vector3.left);
        RaycastHit2D hit = Physics2D.Raycast(outPoint.position, directionToShotTheRaycast, lightLenght, targetMask);
        lineRenderer.enabled = true;
        
        Debug.DrawRay(outPoint.position, directionToShotTheRaycast, Color.green, 10f, true);
        lineRenderer.SetPosition(0, outPoint.position);
        
        if (hit.collider != null)
        {
            if (!startPlayingParticles)
            {
                startPlayingParticles = true;
                hitParticles.Play(true);
            }
            if (hit.collider.CompareTag("Goal"))
            {
                goalAnimator.SetBool("Break", true);
            }
            float distance = ((Vector2)hit.point - (Vector2)outPoint.position).magnitude;
            lineRenderer.SetPosition(1, hit.point);
            Mirror reflectedMirror = hit.collider.GetComponent<Mirror>();
            if (reflectedMirror != null)
            {
                reflectedMirror.GetComponent<Ikillable>().Hit(transform);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, outPoint.position + directionToShotTheRaycast * lightLenght);
            startPlayingParticles = false;
            hitParticles.Stop(true);
        }
    }

    public void UnHit(Transform player)
    {
        mirrorStates = MirrorStates.COOLING_TIME;
    }
    enum MirrorStates
    {
        RECEIVING_LIGHT,
        REFLECTING,
        IDLE,
        COOLING_TIME
    }
    public enum MirrorPosition
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        UPL,
        DOWNL,
        RIGHTL,
        LEFTL
        
    }
}
