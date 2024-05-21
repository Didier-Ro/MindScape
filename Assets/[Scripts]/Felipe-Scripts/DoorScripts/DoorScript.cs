using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform door;
    public float doorSpeed = 1f;
    public int conditionId;
    public static DoorScript Instance { get; private set; }
    public Transform openTransform;
    public Transform closeTransform;
    Vector3 targetPosition;
    float time;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        targetPosition = closeTransform.position;
    }

    void Update()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId) && door.position != targetPosition)
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