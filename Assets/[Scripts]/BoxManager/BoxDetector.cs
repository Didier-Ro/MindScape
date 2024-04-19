using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    [SerializeField] private Vector2 spawnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            BoxCollider2D collider = collision.GetComponent<BoxCollider2D>();
            BoxCollider2D childrenCollider = collision.GetComponentInChildren<BoxCollider2D>();
            collider.enabled = false;
            childrenCollider.enabled = false;
            PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Box, spawnPos, new Vector3(0, 0, 0));
        }
    }
}
