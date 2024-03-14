using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTp : MonoBehaviour
{
    [SerializeField] private Transform teleportA = default;
    [SerializeField] private Transform teleportB = default;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            
            other.transform.position = teleportB.position;
        }
        
    }
}
