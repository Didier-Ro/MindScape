using System;
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
    [SerializeField] private GameObject doorToUnlock;
    [SerializeField] private Transform[] hitPositions;
    private Vector2 directionToShotTheRaycast;
    private float parentOffset;
    private float initialAngleRange;
    private float mediumAngleRange;
    private float upperAngleRange;
    private bool startPlayingParticles;

    [SerializeField] private MirrorPosition _mirrorPosition;


    private void Start()
    {
        ChangeHitPosition();
    }

    private void ChangeHitPosition()
    {
        Vector2 positionsToSpawn = Vector2.zero;
        switch (_mirrorPosition)
        {
            case MirrorPosition.UP:
                positionsToSpawn.x = 0;
                positionsToSpawn.y = 2;
                directionToShotTheRaycast = Vector2.right;
                mediumAngleRange = 90;
                break;
            case MirrorPosition.DOWN:
                positionsToSpawn.x = 1;
                positionsToSpawn.y = 3;
                directionToShotTheRaycast = Vector2.left;
                mediumAngleRange = 270;
                break;
            case MirrorPosition.RIGHT:
                positionsToSpawn.x = 2;
                positionsToSpawn.y = 1;
                directionToShotTheRaycast = Vector2.down;
                mediumAngleRange = 0;
                break;
            case MirrorPosition.LEFT:
                positionsToSpawn.x = 3;
                positionsToSpawn.y = 0;
                directionToShotTheRaycast = Vector2.up;
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
            MirrorProjection();
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
                doorToUnlock.SetActive(false);
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
            lineRenderer.SetPosition(1, outPoint.position + outPoint.TransformDirection(Vector3.left) * lightLenght);
            startPlayingParticles = false;
            hitParticles.Stop(true);
        }
    }

    public void UnHit(Transform player)
    {
        return;
    }
    
    public enum MirrorPosition
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }
}
