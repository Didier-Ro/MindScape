using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private SpriteRenderer doorSprite;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private bool boxOnButton = false;
    [SerializeField] private bool doorIsOpen = false;
    [SerializeField] private int conditionId;
    public int holeNumbers;
    public int buttonNumbers;
    [SerializeField] private byte buttonCounter = 0;
    private byte holeCounter = 0;
    private float delayStart = 3;


    private void Start()
    {
        delayStart *= 60;
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           Destroy(gameObject);
        }
    }
    
    private IEnumerator CoroutineToReturnCamera(bool isOpen)
    {
        
        for (int i = 0; i < delayStart; i++)
        {
            yield return null; 
        }
        CameraManager.instance.ChangeCameraToThePlayer();
        if (isOpen)
        {
            if (doorSprite != null)
                doorSprite.enabled = false;
            
            if (doorCollider != null)
                doorCollider.enabled = false;
        }
        else
        {
            if (doorSprite != null)
                doorSprite.enabled = true;

            if (doorCollider != null)
                doorCollider.enabled = true;
        }
    }

    private void OpenDoor()
    {
        StartCoroutine(CoroutineToReturnCamera(true));
    }

    private void CloseDoor()
    {
        StartCoroutine(CoroutineToReturnCamera(false));
    }

    public void IncreaseCounter()
    {
        buttonCounter++;
        if (buttonCounter >= buttonNumbers)
        {
            CameraManager.instance.ChangeCameraToAnObject(gameObject);
            OpenDoor();
        }
    }

    public void IncreaseHoleCounter()
    {
        holeCounter++;
    }

    public byte ReturnHoleCounter()
    {
        return holeCounter;
    }

    public void DecreaseCounter()
    {
        buttonCounter--;
        if (buttonCounter < buttonNumbers)
        {
            CloseDoor();
        }
    }
    
    public byte ReturnCounter()
    {
        return buttonCounter;
    }
}
