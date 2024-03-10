using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour, Istepable
{
    [SerializeField] private Dialog dialog;
    
    public void Activate()
    {
        Debug.Log("Reading");
        StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
    }

    public void Deactivate()
    {
        Debug.Log("Exit");
      
    }
}
