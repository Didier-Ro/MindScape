using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetector : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private BoxCollider2D colliderParent;
    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private TYPE_DETECTOR typeDetector;
    [SerializeField] private Doors doors;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            if (typeDetector == TYPE_DETECTOR.HOLE)
            {
                Transform parentTransform = collision.transform.parent;
                GameObject parent = parentTransform.gameObject;        
                colliderParent = parent.GetComponent<BoxCollider2D>();
                colliderParent.enabled = false;
                GameObject obj = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
                gameObject.SetActive(false);
            }
            else if (typeDetector == TYPE_DETECTOR.BUTTON)
            { 
                doors.IncreaseCounter();
                if (doors.ReturnCounter() != 2)
                {
                    Transform parentTransform = collision.transform.parent;
                    GameObject parent = parentTransform.gameObject;        
                    colliderParent = parent.GetComponent<BoxCollider2D>();
                    colliderParent.enabled = false;
                    GameObject obj = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
                    gameObject.SetActive(false);
                }
            }
            else if (typeDetector == TYPE_DETECTOR.UNIQUE)
            {
                doors.IncreaseCounter();
              
                    Transform parentTransform = collision.transform.parent;
                    GameObject parent = parentTransform.gameObject;        
                    colliderParent = parent.GetComponent<BoxCollider2D>();
                    colliderParent.enabled = false;
                    gameObject.SetActive(false);
            }
            
        }
    }
}

public enum TYPE_DETECTOR
{
    HOLE,
    BUTTON,
    UNIQUE
}


