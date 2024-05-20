using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTriggerBox : MonoBehaviour, IBoxInteraction
{
    [SerializeField] private int conditionId;
    [SerializeField] private float distanceToFly = 4f;
    public OBJECT_TYPE _objectTypeIsSpawned;
    public Sprite initialSprite;
    public Sprite finalSprite;
    private Vector2 directionToSpawnBox;
    private SpriteRenderer _spriteRenderer;
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
            _spriteRenderer = GetComponent<SpriteRenderer>();
            collider2D = GetComponent<Collider2D>();
        }
    }

    public void Activate(GameObject boxToDeactivate)
    {
        directionToSpawnBox = boxToDeactivate.GetComponent<BoxFalling>().finalPoint;
        boxToDeactivate.SetActive(false);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        StartCoroutine(ChangingFirstSprite());
        GameObject box = PoolManager.GetInstance().GetPooledObject(_objectTypeIsSpawned, transform.position, Vector2.zero);
        box.GetComponent<FlyingBox>().finalPoint = directionToSpawnBox;
        box.GetComponent<FlyingBox>().GetPositionToMove(directionToFly, distanceToFly);
        collider2D.enabled = true;
        CameraManager.instance.ChangeCameraToAnObject(box);
    }

    private IEnumerator ChangingFirstSprite()
    {
        yield return new WaitForSeconds(1.25f);
        _spriteRenderer.sprite = finalSprite;
        yield return new WaitForSeconds(0.25f);
        _spriteRenderer.sprite = initialSprite;
    }
    
    
    public void Deactivate()
    {
       
    }

}
