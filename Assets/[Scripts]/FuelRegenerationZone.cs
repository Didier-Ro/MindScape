using UnityEngine;
using UnityEngine.UI;

public class FuelRegenerationZone : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float regenerationValue = 100f;
    [SerializeField] private GameObject interactInstruction = null;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            interactInstruction.SetActive(true);
            // Check for the "F" key press
            if (InputManager.GetInstance().InteractInput())
            {
                // Set the slider value to the regeneration value
                Flashlight.GetInstance().currentSliderValue = regenerationValue;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactInstruction.SetActive(false);
    }
}
