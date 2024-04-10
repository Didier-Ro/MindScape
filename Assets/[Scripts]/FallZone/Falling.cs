using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Falling : MonoBehaviour
{
    [SerializeField] private float fallingDuration;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Light2D flasLight;
    [SerializeField] private Light2D wallFlaslight;
    [SerializeField] private HealthController healthController;

    private float minSpriteSize = 0.0f;
    private float totalSpriteSize = 0.0f;
    private float totalAlpha;
    private float totalRadious;
    private int frame = 60;
    private bool isFalling;

    void Start()
    {
        SubscribeToGameManagerGameState();
        FallingSetUp();
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            PlayerFalling();
        }
    }

    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
            OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        }
    }

    private void OnGameStateChange (GAME_STATE _newGameState)
    {
        if (_newGameState == GAME_STATE.FALLING)
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

    private void PlayerFalling()
    {
        playerSprite.size -= new Vector2(totalSpriteSize, totalSpriteSize);
        //playerSprite.material.color -= new Color(1, 1, 1, totalAlpha);
        flasLight.pointLightInnerRadius -= totalRadious;
        wallFlaslight.pointLightInnerRadius -= totalRadious;

        if (playerSprite.size.x <= minSpriteSize && playerSprite.size.y <= minSpriteSize)
        {
            playerSprite.size = new Vector2(minSpriteSize, minSpriteSize);
            playerSprite.material.color = new Color(1, 1, 1, minSpriteSize);
            flasLight.pointLightInnerRadius = minSpriteSize;
            wallFlaslight.pointLightInnerRadius = minSpriteSize;
            healthController.currentPlayerHealth -= 100;
            healthController.PlayerTakeDamage();
        }
    }
}
