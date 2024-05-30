using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject Tutorial;
    public int conditionId;
    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(conditionId))
        {
           Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (Tutorial != null)
            {
                Tutorial.SetActive(true);
            }
        }
    }
}
