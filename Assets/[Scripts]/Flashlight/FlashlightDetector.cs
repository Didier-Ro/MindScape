using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightDetector : MonoBehaviour
{
    #region Singleton
    private static FlashlightDetector Instance;
    public static FlashlightDetector GetInstance()
    {
        return Instance;
    }
    #endregion

    [SerializeField] private Light2D flashlight;

    public Action<LIGHT_ENERGY_STATE> OnLightEnergyChange;
    public LIGHT_ENERGY_STATE lightEnergyState = LIGHT_ENERGY_STATE.OFF;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (flashlight.isActiveAndEnabled)
        {
            ChangeEnergyState(LIGHT_ENERGY_STATE.ON);
        }
        else
        {
            ChangeEnergyState(LIGHT_ENERGY_STATE.OFF);
        }
    }

    public void ChangeEnergyState(LIGHT_ENERGY_STATE _energyState)
    {
        lightEnergyState = _energyState;

        if (OnLightEnergyChange != null)
        {
            OnLightEnergyChange.Invoke(lightEnergyState);
        }
    }

    public LIGHT_ENERGY_STATE GetLightEnergyState()
    {
        return lightEnergyState;
    }
}

public enum LIGHT_ENERGY_STATE
{
    ON,
    OFF
}