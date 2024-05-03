using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTp : MonoBehaviour
{
    [SerializeField] private Transform teleportA = default;
    [SerializeField] private Transform teleportB = default;
    [SerializeField] private GameObject compatibleBox = default;
    [SerializeField] private bool isInteracting = false;
    [SerializeField] private bool canInteract;

    private void FixedUpdate()
    {
        TeleportBox();
    }

    public void TeleportBox()
    {
        if (canInteract && InputManager.GetInstance().InteractInput())
        {
            compatibleBox.transform.position = teleportB.position;
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == ("Player") && IsInTp.GetInstance().SetBoxState())
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.tag == ("Player"))
        {
           
            canInteract = false;
            isInteracting = false;
        }
    }
}
