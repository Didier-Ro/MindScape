using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInTp : MonoBehaviour
{
    private bool isOnTp;
    
    #region Singletone
    private static IsInTp Instance;
    public static IsInTp GetInstance() 
    { 
        return Instance;
    }
    #endregion

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

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.tag == ("Box"))
        {
            isOnTp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.tag == ("Box"))
        {
            isOnTp = false;
        }
    }

    public bool SetBoxState()
    {
        return isOnTp;
    }
}
