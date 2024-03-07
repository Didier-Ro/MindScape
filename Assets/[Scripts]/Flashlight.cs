using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light2D flashlight;
    private bool flashing = false;

    // Start is called before the first frame update
    void Start()
    {
        CircleLight();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Flashing();
        }

        if (!flashing)
        {
            CircleLight();
        }
        else
        {
            ConcentrateLight();
        }
    }

    private void CircleLight() 
    {
        flashlight.intensity = 0.7f;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 0.24f;
        flashlight.pointLightInnerAngle = 360;
        flashlight.pointLightOuterAngle = 360;
    }

    private void ConcentrateLight() 
    {
        flashlight.intensity = 1;
        flashlight.pointLightOuterRadius = Mathf.Lerp(3,6, 1);
        flashlight.pointLightInnerRadius = 0.24f;
        flashlight.pointLightInnerAngle = Mathf.Lerp(360, 26, 1);
        flashlight.pointLightOuterAngle = Mathf.Lerp(360, 26, 1);
    }

    private void Flashing()
    {
        flashing = !flashing;
    }
}
