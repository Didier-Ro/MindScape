using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner Instance;

    [SerializeField] private DoorScript doorScript;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static EnemySpawner getInstance()
    {
        return Instance;
    }

    [SerializeField] private Transform[] enemiesSpawns;
    public List<GameObject> enemiesActive = new List<GameObject>();
    [SerializeField] private int maxRounds;
    public GameObject target;
    private int actualRound = 1;

    public void SpawnRound(int _enemiesUWantToSpawn, OBJECT_TYPE _enemyType)
    {
        for (int i = 0; i < _enemiesUWantToSpawn; i++)
        {
            GameObject enemy = PoolManager.GetInstance().GetPooledObject(_enemyType, enemiesSpawns[i].position, Vector3.zero);
            if (_enemyType == OBJECT_TYPE.EnemyChase)
            {
                enemy.GetComponent<Enemy>().AssignTarget(target);
            }
            enemiesActive.Add(enemy);
        }
    }

    public void SpawnRoundEnemy()
    {
        if (actualRound < maxRounds)
        {
            enemiesActive.Clear();
            for (int i = 0; i < actualRound; i++)
            {
                GameObject enemy = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.EnemyChase, enemiesSpawns[i].position, Vector3.zero);
                enemy.GetComponent<Enemy>().AssignTarget(target);
                enemiesActive.Add(enemy);
            }
        }
        CheckArray();
    }

    public void CheckArray()
    {
        if (enemiesActive.Count == 0)
        {
            if (doorScript != null)
            {
                doorScript.isUnlocked = true;
            }
            if (doorScript2 != null)
            {
                doorScript2.isUnlocked = true;
            }

            PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Key, transform.position, Vector3.zero);
        }
    }
}