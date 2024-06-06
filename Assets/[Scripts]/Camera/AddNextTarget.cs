using UnityEngine;

public class AddNextTarget : MonoBehaviour
{
   public GameObject nextTarget;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         if (nextTarget == null)
         {
            CameraManager.instance.ChangeTargetCamera(null);
         }
         else
         {
            CameraManager.instance.ChangeTargetCamera(nextTarget);
         }
         Destroy(this);
      }
   }
}
