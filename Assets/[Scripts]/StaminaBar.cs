using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Slider slider; // Si todavía quieres mantener el slider por algún motivo
    public Image[] dashImages; // Imágenes de la bota iluminada.
    public Image[] greyDashImages; // Imágenes de la bota gris.
    public float regenSpeed = 1f;
    public float delayBeforeRefill = 2f;
    public float maxStamina = 100f;
    public int dashCount = 2; // Número de dashes disponibles.
    private float currentStamina;
    private int currentDashCount;
    private bool isRegenerating;

    private void Start()
    {
        currentStamina = maxStamina;
        currentDashCount = dashCount;
        UpdateDashImages();

        if (slider != null)
        {
            slider.maxValue = maxStamina;
            slider.value = currentStamina;
        }
    }

    private void Update()
    {
        if (isRegenerating)
        {
            RegenerateStamina();
        }
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += regenSpeed * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

            if (slider != null)
            {
                slider.value = currentStamina;
            }

            if (currentStamina == maxStamina)
            {
                currentDashCount = dashCount;
                isRegenerating = false;
                UpdateDashImages();
            }
        }
    }

    public void UseStamina(float amount)
    {
        if (currentDashCount > 0)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            currentDashCount--;
            UpdateDashImages();

            if (slider != null)
            {
                slider.value = currentStamina;
            }

            if (currentDashCount == 0)
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
        for (int i = 0; i < dashCount; i++)
        {
            if (i < currentDashCount)
            {
                dashImages[i].enabled = true;
                greyDashImages[i].enabled = false;
            }
            else
            {
                dashImages[i].enabled = false;
                greyDashImages[i].enabled = true;
            }
        }
    }

    public void SetMaxStamina(float stamina)
    {
        maxStamina = stamina;
        if (slider != null)
        {
            slider.maxValue = stamina;
        }
        currentStamina = stamina;
        UpdateDashImages();
    }

    public void SetStamina(float stamina)
    {
        currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        if (slider != null)
        {
            slider.value = currentStamina;
        }
        UpdateDashImages();
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }
}