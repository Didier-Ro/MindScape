using UnityEngine;

public class FlyingTriggerBox : MonoBehaviour, IBoxInteraction
{
    [SerializeField] private int conditionId;
    [SerializeField] private float distanceToFly = 4f;
    public OBJECT_TYPE _objectTypeIsSpawned;
    public DirectionToFly directionToFly = DirectionToFly.Up;
    private Collider2D collider2D;

    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
            PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector3.zero);
            Destroy(gameObject);
        }
        else
        {
            collider2D = GetComponent<Collider2D>();
        }
    }

    public void Activate(GameObject boxToDeactivate)
    {
        boxToDeactivate.SetActive(false);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        GameObject box = PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector2.zero);
        box.GetComponent<FlyingBox>().GetPositionToMove(directionToFly, distanceToFly);
        collider2D.enabled = true;
        CameraManager.instance.ChangeCameraToAnObject(box);
    }
    
    public void Deactivate()
    {
       
    }

}
