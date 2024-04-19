using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    [SerializeField] private BoxCollider2D colliderParent;
    [SerializeField] private Vector2 spawnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            Transform parentTransform = collision.transform.parent;
            GameObject parent = parentTransform.gameObject;        
            colliderParent = parent.GetComponent<BoxCollider2D>();
            colliderParent.enabled = false;
            PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Box, spawnPos, new Vector3(0, 0, 0));
        }
    }
}
