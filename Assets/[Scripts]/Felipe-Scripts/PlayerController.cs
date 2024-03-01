using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deadly"))
        {
            Die();
        }
    }

    public void Die()
    {
        RespawnAtCheckpoint();
    }

    private void RespawnAtCheckpoint()
    {
        Vector3 checkpointPosition = GameManagerCheckpoints.instance.GetCheckpoint();

        transform.position = checkpointPosition;
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
