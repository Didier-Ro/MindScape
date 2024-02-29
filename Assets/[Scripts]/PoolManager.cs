using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    private List<GameObject> _pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject[] objectsToSpawn;
  
    public static PoolManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
  
   
    public GameObject GetPooledObject(OBJECT_TYPE _type, Vector2 coordinateToSpawn, Vector3 rotation)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                if (_pooledObjects[i].GetComponent<ObjectType>().GetObjectType() == _type )
                {
                    _pooledObjects[i].transform.position = coordinateToSpawn;
                    _pooledObjects[i].SetActive(true);
                    return _pooledObjects[i];
                }
            }
        }

        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            if (objectsToSpawn[i].GetComponent<ObjectType>().GetObjectType() == _type)
            {
                GameObject currentBullet = Instantiate(objectsToSpawn[i],
                    coordinateToSpawn, Quaternion.Euler(rotation));
                _pooledObjects.Add(currentBullet);
                return currentBullet;
            }
        }
        return null;
    }
   
}

public enum OBJECT_TYPE
{
    EnemyChase,
    LanternEnemies,
    Spawn
}