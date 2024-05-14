using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Falling : MonoBehaviour
{
    [SerializeField] private float fallingDuration;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Light2D flasLight;
    [SerializeField] private Light2D wallFlaslight;
    [SerializeField] private HealthController healthController;

    [SerializeField] private DashController dashController;

    [SerializeField] private Vector3 finalPlayerSpawnPosition;
    [SerializeField] private PlayerRespawnPositon playerRespawnPositon;
    private Vector2 initialSpriteSpawnPosition;

    private float minSpriteSize = 0.0f;
    private float totalSpriteSize = 0.0f;
    private float totalAlpha;
    private float totalRadious;
    private int frame = 60;
    private bool isFalling;
    private bool canMove;

    public Vector3 fallZonePosition;

    public float distance;
    public float totalDistance;
    void Start()
    {
        SubscribeToPlayerGameState();
        FallingSetUp();
        SetPlayerRespawnPosition(playerRespawnPositon.respawnPositionCheckPoint);
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            PlayerFalling();
        }

        if (canMove)
        {
            MovePlayer();
        }
    }

    public void SetPlayerRespawnPosition(Vector3 pos)
    {
        finalPlayerSpawnPosition = pos;
    }

    private void RespawnPlayer()
    {
        transform.position = finalPlayerSpawnPosition;
        spriteTransform.position = new Vector3(finalPlayerSpawnPosition.x, 27,0);
        spriteTransform.localScale = new Vector3(1, 1, 1);
        initialSpriteSpawnPosition = new Vector3(finalPlayerSpawnPosition.x, 27, 0);
        canMove = true;
    }

    private void MovePlayer()
    {
        float distance = initialSpriteSpawnPosition.y - finalPlayerSpawnPosition.y;
        float destiny = distance / (60 * 1f);

        spriteTransform.position -= new Vector3(0, destiny, 0);

        if(spriteTransform.position.y <= finalPlayerSpawnPosition.y)
        {
            spriteTransform.position = finalPlayerSpawnPosition;
            canMove = false;
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        }
    }

    private void SubscribeToPlayerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        if (PlayerStates.GetInstance() != null)
        {
            PlayerStates.GetInstance().OnPlayerStateChanged += OnPlayerStateChange;
            OnPlayerStateChange(PlayerStates.GetInstance().GetCurrentPlayerState());
        }
    }

    private void OnPlayerStateChange(PLAYER_STATES _newPlayerState)
    {
        if (_newPlayerState == PLAYER_STATES.FALL)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    private void FallingSetUp()
    {
        totalSpriteSize = 1 / (frame * fallingDuration);
        totalAlpha = 1 / (frame * fallingDuration);
        totalRadious = 3 / (frame * fallingDuration);
    }

    public void SetFallZonePosition(Vector3 _position)
    {
        fallZonePosition = _position;
        isFalling = true;
    }

    private void PlayerFalling()
    {
        dashController.StopAllCoroutines();
        spriteTransform.localScale -= new Vector3(totalSpriteSize, totalSpriteSize);
        //playerSprite.material.color -= new Color(1, 1, 1, totalAlpha);
        flasLight.pointLightInnerRadius -= totalRadious;
        wallFlaslight.pointLightInnerRadius -= totalRadious;

        if (spriteTransform.localScale.x <= minSpriteSize && spriteTransform.localScale.y <= minSpriteSize)
        {
            isFalling = false;
            spriteTransform.localScale = new Vector3(minSpriteSize, minSpriteSize);
            playerSprite.material.color = new Color(1, 1, 1, minSpriteSize);
            flasLight.pointLightInnerRadius = minSpriteSize;
            wallFlaslight.pointLightInnerRadius = minSpriteSize;
            healthController.PlayerTakeDamage(50f);
            RespawnPlayer();
        }
    }
}
