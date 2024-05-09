using System;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealthController = other.gameObject.GetComponent<HealthController>();
            playerHealthController.PlayerTakeDamage(enemyDamage);
        }
    }
}
