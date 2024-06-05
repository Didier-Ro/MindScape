using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Image[] dashImages;
    public Image[] greyDashImages;
    public float regenSpeed = 1f;
    public float delayBeforeRefill = 2f;
    public float maxStamina = 100f;
    public float dashStaminaCost = 50f;
    private float currentStamina;
    private int currentDashIndex;
    private bool isRegenerating;

    private void Start()
    {
        currentStamina = maxStamina;
        currentDashIndex = dashImages.Length - 1;
        UpdateDashImages();
    }

    private void FixedUpdate()
    {
        if (GameManager.GetInstance().GetCurrentGameState() != GAME_STATE.PAUSE)
        {
            if (isRegenerating)
            {
                RegenerateStamina();
            }
        }
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += regenSpeed * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            float dashThreshold = maxStamina / dashImages.Length;
            int newDashIndex = Mathf.FloorToInt(currentStamina / dashThreshold);

            if (newDashIndex > currentDashIndex)
            {
                currentDashIndex = newDashIndex;
                UpdateDashImages();
            }

            if (currentDashIndex >= 0 && currentDashIndex < dashImages.Length)
            {
                float fillAmount = (currentStamina % dashThreshold) / dashThreshold;
                dashImages[currentDashIndex].fillAmount = fillAmount;
            }
        }
        else
        {
            isRegenerating = false;
        }
    }

    public void UseStamina(float amount)
    {
        if (currentDashIndex >= 0)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            if (currentDashIndex < dashImages.Length)
            {
                dashImages[currentDashIndex].fillAmount = 0f;
            }
            currentDashIndex--;
            UpdateDashImages();

            if (!isRegenerating)
            {
                StartCoroutine(RefillStaminaAfterDelay());
            }
        }
    }

    private IEnumerator RefillStaminaAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRefill);
        isRegenerating = true;
    }

    private void UpdateDashImages()
    {
        for (int i = 0; i < dashImages.Length; i++)
        {
            if (i <= currentDashIndex)
            {
                dashImages[i].fillAmount = 1f;
                dashImages[i].enabled = true;
            }
            else
            {
                dashImages[i].enabled = false;
            }
            greyDashImages[i].enabled = true;
        }
    }

    public void SetMaxStamina(float stamina)
    {
        maxStamina = stamina;
        currentStamina = stamina;
        UpdateDashImages();
    }

    public void SetStamina(float stamina)
    {
        currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        UpdateDashImages();
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }
}