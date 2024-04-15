using System;
using UnityEngine;

public class ActivateZone : MonoBehaviour
{
   private bool canActivate = true;
   [SerializeField] private GameObject gameObjectToActivate;
   


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
