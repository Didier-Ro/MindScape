using System;
using System.Collections;
using UnityEngine;

public class FlyingBox : MonoBehaviour
{
   [SerializeField] private float distanceToMove;
   [SerializeField] private float sizeToScale;
   [SerializeField] private float delayStart;
   private float speedPerFrame = default;
   private int frameCounter = 0;
   public float timeToReachPointInSeconds = 1;
   private int finalFramesToReachPoint = default;
   private Vector2 finalPosition;
   private bool reachToThePoint = true;
   private float speedFrameToScale;
   
   
   private void Start()
   {
      finalFramesToReachPoint = (int)timeToReachPointInSeconds * 60;
      finalPosition = new Vector2(transform.position.x, transform.position.y + distanceToMove);
      StartCoroutine(CoroutineToStartFlying());
   }
   private void FixedUpdate()
   {
      if (!reachToThePoint)
      {
         FlyingProcess();
         RescalingBox();
      }
   }

   private void RescalingBox()
   {
      speedFrameToScale = sizeToScale / (finalFramesToReachPoint / 2f);
       if (frameCounter <= finalFramesToReachPoint / 2)
       {
          transform.localScale += new Vector3(speedFrameToScale, speedFrameToScale, speedFrameToScale);
       }
       else if(frameCounter <= finalFramesToReachPoint)
       {
          transform.localScale -= new Vector3(speedFrameToScale, speedFrameToScale, speedFrameToScale);
       }
     
   }

   private IEnumerator CoroutineToStartFlying()
   {
      delayStart *= 60;
      for (int i = 0; i < delayStart; i++)
      {
         yield return null; 
      }
      reachToThePoint = false;
   }

   private void FlyingProcess()
   {
      speedPerFrame = distanceToMove / (timeToReachPointInSeconds * 60);
      if (frameCounter <= finalFramesToReachPoint)
      {
         transform.Translate(Vector2.up * speedPerFrame);
         frameCounter++;
      }
      else
      {
         transform.position = finalPosition;
         reachToThePoint = true;
         PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Box, transform.position, Vector3.zero);
         CameraManager.instance.ChangeCameraToThePlayer();
         gameObject.SetActive(false);
      }
   }
}
