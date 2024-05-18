using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionCheckPoint : MonoBehaviour
{
    [SerializeField] private PlayerRespawnPositon playerRespawnPositon;
    private Collider2D checkCollider2D;
    void Start()
    {
        checkCollider2D = GetComponent<Collider2D>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRespawnPositon.SetCheckPointSpawnPosition(transform.position);
            collision.gameObject.GetComponent<Falling>().SetPlayerRespawnPosition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        checkCollider2D.enabled = false;
    }
}