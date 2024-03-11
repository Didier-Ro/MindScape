using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private DashController _dashController;
    public float regenSpeed = 1f; // Stamina regeneration speed per frame
    public float shiftStaminaCost = 25f; // Stamina cost when using Shift key
    public float delayBeforeRefillInFrames = 90f; // Delay before stamina starts refilling after it's used, in frames

    private float currentStamina;
    private int framesSinceLastUse = 0;

    private void Start()
    {
        currentStamina = slider.maxValue; // Initialize stamina to max at start
    }

    private void FixedUpdate()
    {
        RegenerateStamina();
        framesSinceLastUse++; // Increment frames count since last use in each fixed update
    }

    private void RegenerateStamina()
    {
        if (currentStamina < slider.maxValue && framesSinceLastUse > delayBeforeRefillInFrames)
        {
            currentStamina += regenSpeed; // Increase stamina at regeneration speed per frame
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
        if (currentStamina >= shiftStaminaCost)
        {
            currentStamina -= shiftStaminaCost; // Reduce stamina when Shift key is pressed
            currentStamina = Mathf.Clamp(currentStamina, 0f, slider.maxValue); // Ensure stamina doesn't go below 0
            slider.value = currentStamina; // Update stamina bar value
            framesSinceLastUse = 0; // Reset frames count since last use
        }
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
