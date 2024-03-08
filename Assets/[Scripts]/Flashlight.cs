using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private float energy = 100f; // Initial energy value
    private bool flashing = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeFlashlight();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateEnergyUI();
    }

    // Initialize the flashlight settings
    private void InitializeFlashlight()
    {
        CircleLight();
        energy = slider.maxValue;
    }

    // Handle input to toggle flashlight mode
    private void HandleInput()
    {
        if (InputManager.GetInstance().IsOn())
        {
            ToggleFlashing();
        }

        if (!flashing)
        {
            CircleLight();
        }
        else
        {
            ConcentrateLight();
        }
        // Reduce energy based on flashlight mode
        ReduceEnergy();
    }

    // Update the energy UI slider
    private void UpdateEnergyUI()
    {
        slider.value = energy;
    }

    // Set flashlight settings for circle light mode
    private void CircleLight()
    {
        flashlight.intensity = 0.7f;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 0.24f;
        flashlight.pointLightInnerAngle = 360;
        flashlight.pointLightOuterAngle = 360;
    }

    // Set flashlight settings for concentrated light mode
    private void ConcentrateLight()
    {
        flashlight.intensity = 1;
        flashlight.pointLightOuterRadius = Mathf.Lerp(3, 6, 1);
        flashlight.pointLightInnerRadius = 0.24f;
        flashlight.pointLightInnerAngle = Mathf.Lerp(360, 26, 1);
        flashlight.pointLightOuterAngle = Mathf.Lerp(360, 26, 1);
    }

    // Toggle between flashlight modes
    public void ToggleFlashing()
    {
        flashing = !flashing;
    }

    // Reduce energy based on flashlight mode
    private void ReduceEnergy()
    {
        if (!flashing)
        {
            energy += Time.deltaTime / 2; // Reduce energy slowly
        }
        else
        {
            energy -= Time.deltaTime / 3; // Reduce energy faster
        }

        // Clamp energy to ensure it stays within valid range
        energy = Mathf.Clamp(energy, 0f, slider.maxValue);
    }

    // Function to get current energy level
    public float GetEnergy()
    {
        return energy;
    }
}
