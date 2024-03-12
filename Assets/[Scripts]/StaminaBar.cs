using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private DashController _dashController;
    public float regenSpeed = 1f; // Stamina regeneration speed per second
    public float shiftStaminaCost = 25f; // Stamina cost when using Shift key
    public float delayBeforeRefill = 1.5f; // Delay before stamina starts refilling after it's used

    private float currentStamina;
    private float refillTimer = 0f;

    private void Start()
    {
        currentStamina = slider.maxValue; // Initialize stamina to max at start
    }

    private void Update()
    {
        RegenerateStamina();
       //UseStamina();
    }

    private void RegenerateStamina()
    {
        if (currentStamina < slider.maxValue && Time.time > refillTimer)
        {
            currentStamina += regenSpeed * Time.deltaTime; // Increase stamina at regeneration speed
            currentStamina = Mathf.Clamp(currentStamina, 0f, slider.maxValue); // Ensure stamina does not exceed maximum
            slider.value = currentStamina; // Update stamina bar value
        }
        if (currentStamina >= slider.maxValue)
        {
            _dashController.UnlockDash();
        }
    }

    public void UseStamina()
    {
            currentStamina -= shiftStaminaCost; // Reduce stamina when Shift key is pressed
            currentStamina = Mathf.Clamp(currentStamina, 0f, slider.maxValue); // Ensure stamina doesn't go below 0
            slider.value = currentStamina; // Update stamina bar value
            refillTimer = Time.time + delayBeforeRefill; // Set the refill timer
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
        currentStamina = stamina; // Ensure current stamina matches maximum
    }

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
        currentStamina = stamina; // Update current stamina
    }
}