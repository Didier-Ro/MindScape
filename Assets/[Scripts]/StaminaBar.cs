using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    public float regenSpeed = 1f; // Velocidad de regeneración de stamina por segundo
    public float delayBeforeRefill = 2f; // Tiempo de espera antes de que la stamina comience a rellenarse después de su uso
    public float maxStamina = 100f; // Stamina máxima
    private float currentStamina;

    private void Start()
    {
        currentStamina = maxStamina; // Inicializa la stamina al máximo al inicio
        slider.maxValue = maxStamina; // Establece el valor máximo del slider de stamina
        slider.value = currentStamina; // Actualiza el valor del slider de stamina
    }

    private void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += regenSpeed * Time.deltaTime; // Aumenta la stamina a la velocidad de regeneración
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Asegura que la stamina no exceda el máximo
            slider.value = currentStamina; // Actualiza el valor del slider de stamina
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount; // Reduce la stamina cuando se usa
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Asegura que la stamina no sea menor que cero
        slider.value = currentStamina; // Actualiza el valor del slider de stamina
        StartCoroutine(RefillStaminaAfterDelay());
    }

    private System.Collections.IEnumerator RefillStaminaAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeRefill);
        // Inserta aquí cualquier lógica adicional que necesites después de que la stamina se haya rellenado
    }

    public void SetMaxStamina(float stamina)
    {
        maxStamina = stamina;
        slider.maxValue = stamina;
        currentStamina = stamina; // Asegura que la stamina actual coincida con el máximo
    }

    public void SetStamina(float stamina)
    {
        currentStamina = stamina;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Asegura que la stamina no exceda el máximo
        slider.value = currentStamina; // Actualiza el valor del slider de stamina
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }
}