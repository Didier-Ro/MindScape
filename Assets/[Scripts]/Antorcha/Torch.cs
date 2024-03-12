using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour, EIstepable, Istepable
{
    [SerializeField] private Light2D ligth2D;
    [SerializeField] private float lightOffTime = 2.5f;
    private float maxIntensity = 1.0f;
    private float minIntensity = 0.0f;
    private float currentIntensity = default;
    private float intensityTimeSpeed  =default;
    private bool isLightOn = true;
    private bool enemyTurningOff = false;

    private int frames = 60;
    private float totalIntensityValue = default;
    
    void Start()
    {
        totalIntensityValue = maxIntensity - minIntensity;
        intensityTimeSpeed = totalIntensityValue / (frames * lightOffTime);
    }

    private void FixedUpdate()
    {
        if (enemyTurningOff)
        {
            ReduceLightIntensity();
        }
    }

    public void EActivate()
    {
        Debug.Log("Entra");
        enemyTurningOff = true;
        
    }

    public void EDeactivate()
    {
        enemyTurningOff = false;
        ligth2D.intensity = currentIntensity;
        
    }

    public bool IsLightOn()
    {
        return isLightOn;
    }

    private void ReduceLightIntensity() 
    {
        ligth2D.intensity -= intensityTimeSpeed;
        currentIntensity = intensityTimeSpeed;
        if (ligth2D.intensity <= minIntensity)
        {
            ligth2D.intensity = 0;
            currentIntensity = minIntensity;
            isLightOn = false;
        }
    }

    public void Activate()
    {
        
    }

    public void Deactivate()
    {
        
    }
}
