using System;
using UnityEngine;

public class Precipice : MonoBehaviour
{
   [SerializeField] private int conditionId;
   [SerializeField] private GameObject boxToSpawnedIsPuzzleIsCompleted;

   private void Start()
   {
      if (GameManager.GetInstance().IsConditionCompleted(conditionId))
      {
          PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.FallingBox, transform.position, Vector3.zero);
          Destroy(gameObject);
      }
   }
}
