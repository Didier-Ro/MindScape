using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjects : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            UIManager.GetInstance().InteractionUI(); 
        }
    }

    public virtual void InteractionResponse()  
    {
        Debug.Log("Action");
    }
}
