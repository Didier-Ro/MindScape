using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelRegenerationZone : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float regenerationValue = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El jugador entro");
            if (InputManager.GetInstance().InteractInput() && Flashlight.GetInstance().currentSliderValue > 100)
            {
                Flashlight.GetInstance().currentSliderValue = regenerationValue;
                Debug.Log("El jugador presiono la interaccion");
            }
        }
    }
}
