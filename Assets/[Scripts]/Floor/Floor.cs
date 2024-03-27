using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, Fstepable
{
    [SerializeField] private FLOOR tFloor;
    

    public void FActivate()
    {
        AudioManager.GetInstance().FloorSound(tFloor);
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