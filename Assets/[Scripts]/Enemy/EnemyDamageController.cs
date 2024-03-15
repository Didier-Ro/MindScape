using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    public float enemyDamage = 50f;
    public HealthController playerHealthController = null;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealthController = other.GetComponent<HealthController>();
            playerHealthController.currentPlayerHealth -= enemyDamage;
            playerHealthController.PlayerTakeDamage();
        }
    }
}
