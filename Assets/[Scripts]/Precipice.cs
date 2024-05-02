using System;
using UnityEngine;

public class Precipice : MonoBehaviour
{
   [SerializeField] private int conditionId;
   [SerializeField] private GameObject boxToSpawnedIsPuzzleIsCompleted;
   private ActivateZone activateZone;

   private void Start()
   {
       activateZone = GetComponent<ActivateZone>();
       
      if (GameManager.GetInstance().IsConditionCompleted(conditionId))
      {
          activateZone.DeactivateCanvas();
          PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.FallingBox, transform.position, Vector3.zero);
          Destroy(gameObject);
      }
   }
}
