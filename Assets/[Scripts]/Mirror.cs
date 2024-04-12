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
    [SerializeField] private float upperAngleRange = -170f;
    [SerializeField] private float lowerAngleRange = -10f;
    private bool startPlayingParticles;


    private void Start()
    {
        lowerAngleRange =  Mathf.Repeat(lowerAngleRange, 360);
        upperAngleRange = Mathf.Repeat(upperAngleRange, 360);
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
        canSeeTarget = angleDegrees > lowerAngleRange && upperAngleRange > angleDegrees;
        return canSeeTarget;
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
    
  /*  private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float lowerAngleRad = Mathf.Deg2Rad * lowerAngleRange;
        float upperAngleRad = Mathf.Deg2Rad * upperAngleRange;
        // Dibujar arco entre los dos ángulos
        for (int i = 0; i <= 50; i++)
        {
            float angle = Mathf.Lerp(lowerAngleRad, upperAngleRad, (float)i / 50);
            Vector3 point = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 1;
            Gizmos.DrawLine(Vector3.zero, point);
        }
    }*/
}
