using System;
using UnityEngine;

public class ActivateZone : MonoBehaviour
{
   private bool canActivate = true;
   [SerializeField] private GameObject gameObjectToActivate;
   private string currentControlScheme;
   [SerializeField] private GameObject[] gameUI;
  
   
   public void DeactivateCanvas()
   {
      gameUI[0].SetActive(false);
      gameUI[1].SetActive(false);
   }
   
   public void SetActiveCanvas()
   {
      if (InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Gamepad")
      {
         gameUI[0].SetActive(false);
         gameUI[1].SetActive(true);
      }
      else if(InputManager.GetInstance().ReturnControlScheme(currentControlScheme) == "Keyboard")
      {
         gameUI[0].SetActive(true);
         gameUI[1].SetActive(false);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         SetActiveCanvas();
      }
       
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         DeactivateCanvas();
      }
   }


   private void OnTriggerStay2D(Collider2D other)
   {
      if (other.CompareTag("Player") && canActivate && InputManager.GetInstance().InteractInput())
      {
         canActivate = false;
         gameObjectToActivate.GetComponent<MovableObject>().GetDirection(other.transform.position);
      }
   }


   public void ActivateBool()
   {
      canActivate = true;
   }
}
