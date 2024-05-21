using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCheckPoint : MonoBehaviour
{
    [SerializeField] private int levelConditionCheck;
    private void Start()
    {
        if (GameManager.GetInstance().IsConditionCompleted(levelConditionCheck))
        {
            PlayerStates.GetInstance().transform.position = CheckpointManager.FindNearestCheckpoint(transform.position);
        }
        else
        {
            GameManager.GetInstance().MarkConditionCompleted(levelConditionCheck);
            PlayerStates.GetInstance().transform.position = transform.position;
            GameManager.GetInstance().SavePlayerPosition(transform.position);
            GameManager.GetInstance().SaveAllData();
        }
    }
}
