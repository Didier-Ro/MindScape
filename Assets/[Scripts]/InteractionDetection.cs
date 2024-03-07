using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetection : MonoBehaviour
{
    bool canInteract = false;
    [SerializeField] private GameObject interactableObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            interactableObject = collision.gameObject;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            canInteract = false;
            interactableObject = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract) 
        {
            interactableObject.GetComponent<InteractableObjects>().InteractionResponse();
            canInteract=false;
        }
    }
}
