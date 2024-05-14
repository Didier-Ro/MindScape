
using UnityEngine;

public class TriggerInitialize : MonoBehaviour
{
    public DoorScript doorScript;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorScript.isUnlocked = false;
            EnemySpawner.getInstance().SpawnRound(5, OBJECT_TYPE.EnemyChase);
           Destroy(gameObject);
        }
    }
}
