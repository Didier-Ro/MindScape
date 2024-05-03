using UnityEngine;

public class Precipice : MonoBehaviour, IBoxInteraction
{
   [SerializeField] private int conditionId;
   public OBJECT_TYPE _objectTypeIsSpawned;

   private void Start()
   {
      if (GameManager.GetInstance().IsConditionCompleted(conditionId))
      {
          PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector3.zero);
          Destroy(gameObject);
      }
   }

   public void Activate(GameObject boxToDeactivate)
   {
       boxToDeactivate.SetActive(false);
       PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
       PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector2.zero);
       gameObject.SetActive(false);
   }

   public void Deactivate()
   {
       
   }
}
