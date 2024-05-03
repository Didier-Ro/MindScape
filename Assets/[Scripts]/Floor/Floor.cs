using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, Fstepable
{
    [SerializeField] private SOUND_TYPE tFloor;
    

    public void FActivate()
    {
        AudioManager.GetInstance().SetSound(tFloor);
    }

    public void FDeactivate()
    {
        
    }
}

public enum FLOOR
{
    WOOD,
    CONCRETE
}