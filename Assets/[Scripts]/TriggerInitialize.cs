
using UnityEngine;

public class TriggerInitialize : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           EnemySpawner.getInstance().SpawnRoundEnemy();
           Destroy(gameObject);
        }
    }
}
