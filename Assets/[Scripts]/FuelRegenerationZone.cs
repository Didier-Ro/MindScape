using UnityEngine;
using UnityEngine.UI;

public class FuelRegenerationZone : MonoBehaviour, Istepable
{
    [SerializeField] private Slider slider;
    [SerializeField] private float regenerationValue = 100f;
    [SerializeField] private GameObject interactInstruction = null;

    public bool isInside = false;

    private void Update()
    {
        Recharge();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.CompareTag("Player"))
        {
            Deactivate();
        }
    }

    void Recharge()
    {
        if (isInside)
        {
            // Check for the "F" key press
            if (InputManager.GetInstance().InteractInput())
            {
                // Set the slider value to the regeneration value
                Flashlight.GetInstance().currentSliderValue = regenerationValue;
            }
        }
    }

    public void Activate()
    {
        isInside = true;
        interactInstruction.SetActive(true);
        Flashlight.GetInstance().ReduceSliderValue(0f);
    }

    public void Deactivate()
    {
        isInside = false;
        interactInstruction.SetActive(false);
        Flashlight.GetInstance().ReduceSliderValue(0.01f);
    }
}
