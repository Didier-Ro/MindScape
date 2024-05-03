using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerFuel : MonoBehaviour
{
    public GameObject playerSpotLight;
    public GameObject light2D;
    public Light2D globalLight;
    public Flashlight playerFlashlight;

    private bool isCoroutineRunning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine(LightIntensityFlicker());
            }
            Flashlight.GetInstance().isInInitialRoom = false;
            playerSpotLight.SetActive(true);
            light2D.SetActive(true);
            playerFlashlight.enabled = true;
        }
    }

    IEnumerator LightIntensityFlicker()
    {
        isCoroutineRunning = true;

        float flickerDuration = 3f;
        float flickerStartTime = Time.time;
        float flickerEndTime = flickerStartTime + flickerDuration;

        while (Time.time < flickerEndTime)
        {
            // Cambia entre 1 y 0.1 cada 5 frames
            if (Time.frameCount % 20 == 0)
            {
                globalLight.intensity = (globalLight.intensity == 1f) ? 0.1f : 1f;
            }
            yield return null;
        }

        // Asegúrate de que la intensidad sea 0.1 al final
        globalLight.intensity = 0.1f;
        isCoroutineRunning = false;
    }


}
