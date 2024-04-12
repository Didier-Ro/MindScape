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
    [SerializeField] private float initialAngleRange;
    [SerializeField] private float upperAngleRange;
    private float parentOffset;
    private bool startPlayingParticles;


    private void Start()
    {
       CheckParentRotation();
    }

    private void CheckParentRotation()
    {
        parentOffset = transform.parent.eulerAngles.z;
        parentOffset = Mathf.Repeat(parentOffset, 360);
        initialAngleRange = Mathf.Repeat(parentOffset + initialAngleRange, 360);
        upperAngleRange = Mathf.Repeat(initialAngleRange + angleRange, 360);
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
        Vector2 direction = player.position - transform.position;
        direction.x = ReduceErrorZero(direction.x);
        direction.y = ReduceErrorZero(direction.y);
        float angleRadians = Mathf.Atan2(direction.y, direction.x);
        float angleDegrees = Mathf.Repeat(angleRadians * Mathf.Rad2Deg, 360);
        if (initialAngleRange < upperAngleRange)
        {
            return canSeeTarget = angleDegrees > initialAngleRange  && upperAngleRange  > angleDegrees;
            Debug.Log(canSeeTarget);
        }
        else
        {
            bool secondSegment = angleDegrees >= initialAngleRange && angleDegrees <= 360;
            bool firstSegment = angleDegrees >= 0 && angleDegrees <= upperAngleRange;
            
          return  canSeeTarget = angleDegrees > initialAngleRange || upperAngleRange  > angleDegrees;
        }
    }

    public void MirrorProjection()
    {
        Vector3 direction = outPoint.TransformDirection(Vector3.left);
        RaycastHit2D hit = Physics2D.Raycast(outPoint.position, direction, lightLenght, targetMask);
        lineRenderer.enabled = true;
        
        Debug.DrawRay(outPoint.position, outPoint.TransformDirection(Vector3.left), Color.green, 10f, true);
        lineRenderer.SetPosition(0, outPoint.position);
        
        if (hit.collider != null)
        {
            if (!startPlayingParticles)
            {
                startPlayingParticles = true;
                hitParticles.Play(true);
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
    
   
}
