using UnityEngine;
using UnityEngine.UI;

public class FuelRegenerationZone : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float regenerationValue = 100f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("entro");
            // Check for the "E" key press
            if (InputManager.GetInstance().InteractInput())
            {
                Debug.Log("El jugador presiono la interaccion");
                // Set the slider value to the regeneration value
                Flashlight.GetInstance().currentSliderValue = regenerationValue;
            }
        }
    }
}
