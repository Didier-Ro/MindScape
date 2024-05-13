using UnityEngine;

public class FlyingTriggerBox : MonoBehaviour, IBoxInteraction
{
    [SerializeField] private int conditionId;
    public OBJECT_TYPE _objectTypeIsSpawned;
    public DirectionToFly directionToFly = DirectionToFly.Up;

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
        GameObject box = PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector2.zero);
        box.GetComponent<FlyingBox>().GetPositionToMove(directionToFly);
        CameraManager.instance.ChangeCameraToAnObject(box);
    }
    
    public void Deactivate()
    {
       
    }

}
