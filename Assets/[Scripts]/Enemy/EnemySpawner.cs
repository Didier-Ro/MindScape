using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner Instance;
    public int conditionId = 6;
    [SerializeField] private DoorScript doorScript;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            
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
    [SerializeField] private GameObject key;
    private bool keyAnimating = false;
    private float keyAnimationTime = 0.5f;
    private float keyAnimationHeight = 0.5f;

    private void Update()
    {
        /*if (keyAnimating)
        {
            float yPos = Mathf.Sin((Time.time / keyAnimationTime) * Mathf.PI) * keyAnimationHeight;
            key.transform.position = new Vector3(key.transform.position.x, yPos, key.transform.position.z);
            if (Time.time >= keyAnimationTime)
            {
                keyAnimating = false;
                key.transform.position = new Vector3(key.transform.position.x, 0f, key.transform.position.z);
            }
        }*/
    }

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
          /*  if (doorScript != null)
            {
                doorScript.isUnlocked = true;
            }*/
            StartKeyAnimation();
        }
    }

    private void StartKeyAnimation()
    {
        keyAnimating = true;
        key.SetActive(true);
    }
}