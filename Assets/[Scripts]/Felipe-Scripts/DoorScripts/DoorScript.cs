using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform door;
    public float doorSpeed = 1f;
    public bool isUnlocked = true;
    public Transform openTransform;
    public Transform closeTransform;
    Vector3 targetPosition;
    float time;

    void Start()
    {
        targetPosition = closeTransform.position;
    }

    void Update()
    {
        if (isUnlocked && door.position != targetPosition)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, time);
            time += Time.deltaTime * doorSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetPosition = openTransform.position;
            time = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetPosition = closeTransform.position;
            time = 0;
        }
    }
}