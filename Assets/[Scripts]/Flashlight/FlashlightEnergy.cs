using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlashLightEnergy", menuName = "ScriptableObjects/FlashLightEnergy")]

public class FlashlightEnergy : ScriptableObject
{
    public float flashlightEnergy;

    public void SetEnergy(float currentEnergy)
    {
        flashlightEnergy = currentEnergy;
    }

    public float GetEnergy()
    {
        return flashlightEnergy;
    }
}
