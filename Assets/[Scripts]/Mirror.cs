using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool startPlayingParticles;
    


    public void Hit()
    {
        Vector3 direction = outPoint.TransformDirection(Vector3.left);
        RaycastHit2D hit = Physics2D.Raycast(outPoint.position, direction, lightLenght, targetMask);
        lineRenderer.enabled = true;
        
        Debug.DrawRay(outPoint.position, outPoint.TransformDirection(Vector3.left), Color.green, 10f, true);
        
        if (hit.collider != null)
        {
            if (!startPlayingParticles)
            {
                startPlayingParticles = true;
                hitParticles.Play(true);
            }

            float distance = ((Vector2)hit.point - (Vector2)transform.position).magnitude;
            lineRenderer.SetPosition(1, new Vector3(distance,0,0));
        Mirror reflectedMirror = hit.collider.GetComponent<Mirror>();
        if (reflectedMirror != null)
        {
            reflectedMirror.GetComponent<Ikillable>().Hit();
        }

        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(lightLenght,0,0));
            startPlayingParticles = false;
            hitParticles.Stop(true);
        }
        
        
    }
}
