using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner Instance;
    
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
    public GameObject CanvasWinner;
    public List<GameObject> enemiesActive = new List<GameObject>();
    [SerializeField] private int maxRounds;
    public GameObject target;
    private int actualRound = 1;
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
        else
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.DEAD);
            CanvasWinner.SetActive(true);
        }
    }

    public void CheckArray()
    {
        if (enemiesActive.Count == 0)
        {
            actualRound++;
            SpawnRoundEnemy();
        }
    }
}