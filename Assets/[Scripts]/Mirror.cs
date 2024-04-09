using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, Ikillable
{

    [SerializeField] private Transform hitPoint;
    [SerializeField] private Transform outPoint;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Hit()
    {
        Vector3 direction = outPoint.TransformDirection(Vector3.left);
        RaycastHit2D hit = Physics2D.Raycast(outPoint.position, direction, 10f, targetMask);
        
        Debug.DrawRay(outPoint.position, outPoint.TransformDirection(Vector3.left), Color.green, 10f, true);
        
        if (hit.collider != null)
        {
        Mirror reflectedMirror = hit.collider.GetComponent<Mirror>();
        if (reflectedMirror != null)
        {
            reflectedMirror.GetComponent<Ikillable>().Hit();
        }

        }
        
        
    }
}
