using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float regenSpeed = 1f;
    public float delayBeforeRefill = 2f;
    public float maxStamina = 100f;
    private float currentStamina;

    private void Start()
    {
        currentStamina = maxStamina;
        slider.maxValue = maxStamina;
        slider.value = currentStamina;
    }

    private void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += regenSpeed * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            slider.value = currentStamina;
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        slider.value = currentStamina;
        StartCoroutine(RefillStaminaAfterDelay());
    }

    private System.Collections.IEnumerator RefillStaminaAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRefill);
    }

    public void SetMaxStamina(float stamina)
    {
        maxStamina = stamina;
        slider.maxValue = stamina;
        currentStamina = stamina;
    }

    public void SetStamina(float stamina)
    {
        currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        slider.value = currentStamina;
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }
}