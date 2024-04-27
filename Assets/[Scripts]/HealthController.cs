using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Player health")]
    public float currentPlayerHealth = 100.0f;
    public float maxPlayerHealth = 100.0f;
    public GameObject gameOverScreen;
    public int regenRate = 1;
    private bool canRegen = false;

    [Header("Images")]
    public Image migraineImage = null;
    public Image migraineFlashImage = null;
    public float hurtTimer = 0.1f;

    [Header("Heal Timer")]
    public float healCooldown = 3.0f;
    public float maxHealCoolDown = 3.0f;
    private bool startCoolDown = false;

    void UpdatePlayerHealth()
    {
        Color imageAlpha = migraineImage.color;
        imageAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        migraineImage.color = imageAlpha;
    }

    public void PlayerTakeDamage()
    {
        if (currentPlayerHealth >= 0)
        {
            canRegen = false;
            UpdatePlayerHealth();
            healCooldown = maxHealCoolDown;
            startCoolDown = true;
            PlayerCameraShake.Instance.ShakeCamera(2f, 0.1f);
        }
        else if (currentPlayerHealth <= 0)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.DEAD);
            currentPlayerHealth = 0;
           // gameOverScreen.SetActive(true);
            Debug.Log("Se murio");
        }
    }

    void Regen()
    {
        if (startCoolDown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCoolDown = false;
            }
        }

        if (canRegen)
        {
            if (currentPlayerHealth <= maxPlayerHealth - 0.01f)
            {
                currentPlayerHealth += Time.deltaTime * regenRate;
                UpdatePlayerHealth();
            }
            else
            {
                currentPlayerHealth = maxPlayerHealth;
                healCooldown = maxHealCoolDown;
                canRegen = false;
            }
        }
    }

    private void Update()
    {
        Regen();
    }
}
