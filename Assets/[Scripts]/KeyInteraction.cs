using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteraction : MonoBehaviour, Istepable
{
    public void Activate()
    {
        Debug.Log("Reading");
    }

    public void Deactivate()
    {
        Debug.Log("Exit");
    }
}