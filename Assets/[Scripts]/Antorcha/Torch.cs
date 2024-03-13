using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour, EIstepable, Istepable
{
    [Header("Light settings")]
    [SerializeField] private Light2D ligth2D;
    [SerializeField] private float lightOffTime = 2.5f;
    [SerializeField] private float lightOnTime = 0.5f;
    [SerializeField] private float maxIntensity = 1.0f;
    [SerializeField] private float minIntensity = 0.0f;
    private float currentIntensity = default;
    private float intensityOffTimeSpeed  =default;
    private float intensityOnTimeSpeed =default;
    private bool isLightOn = true;
    [SerializeField] private bool enemyTurningOff = false;
    private int frames = 60;
    private float totalIntensityValue = default;
    [SerializeField] private bool playerTurningOn = false;
    
    void Start()
    {
        totalIntensityValue = maxIntensity - minIntensity;
        intensityOffTimeSpeed = totalIntensityValue / (frames * lightOffTime);
        intensityOnTimeSpeed = totalIntensityValue / (frames * lightOnTime);
    }

    private void FixedUpdate()
    {
        if (enemyTurningOff)
        {
            ReduceLightIntensity();
        }

        if (playerTurningOn)
        {
            IncreaseLightIntensity();
        }
    }

    public void Activate()
    {
        playerTurningOn = true;
        
    }

    public void Deactivate()
    {
        playerTurningOn = false;
    }

    public void EActivate()
    {
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
        ligth2D.intensity -= intensityOffTimeSpeed;
        currentIntensity = intensityOffTimeSpeed;
        if (ligth2D.intensity <= minIntensity)
        {
            ligth2D.intensity = 0;
            currentIntensity = minIntensity;
            isLightOn = false;
        }
    }

    private void IncreaseLightIntensity()
    {
        ligth2D.intensity += intensityOnTimeSpeed;
        currentIntensity = intensityOnTimeSpeed;
        if (ligth2D .intensity >= maxIntensity)
        {
            ligth2D.intensity = maxIntensity;
            currentIntensity = maxIntensity;
            isLightOn = true;
            playerTurningOn = false;
        }
    }
}
