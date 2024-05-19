using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    private List<GameObject> _pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject[] objectsToSpawn;

    #region Singleton
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
    #endregion

    public GameObject GetPooledObject(OBJECT_TYPE _type, Vector2 coordinateToSpawn, Vector3 rotation)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                ObjectType objectType = _pooledObjects[i].GetComponent<ObjectType>();
                if (objectType != null && objectType.GetObjectType() == _type)
                {
                    _pooledObjects[i].transform.position = coordinateToSpawn;
                    _pooledObjects[i].transform.rotation = Quaternion.Euler(rotation);
                    _pooledObjects[i].SetActive(true);
                    return _pooledObjects[i];
                }
            }
        }

        for (int i = 0; i < objectsToSpawn.Length; i++)
        {
            ObjectType objectType = objectsToSpawn[i].GetComponent<ObjectType>();
            if (objectType != null && objectType.GetObjectType() == _type)
            {
                GameObject currentObject = Instantiate(objectsToSpawn[i], coordinateToSpawn, Quaternion.Euler(rotation));
                _pooledObjects.Add(currentObject);
                return currentObject;
            }
        }

        return null;
    }
}

public enum OBJECT_TYPE
{
    EnemyChase,
    LanternEnemies,
    Spawn,
    FallingBox,
    Box,
    Key,
    FlyingBox,
    ChispasCirculo,
    Estres1,
    Estres1Variant,
    Estres2Variant,
    HumoantorchasVariant,
    Humoantorchaschispas,
    HumoEnemigoVariant,
    HumoEnemigoarriba,
    Pasos,
    Rasgunos,
    Linternacerradaconluz,
    ParituclasCajaCaida,
    ParituclasCajaCaida1,
    ParticulaCajaCaida2,
    Particulallave,
    Particulallave1,
}