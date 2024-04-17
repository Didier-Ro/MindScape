using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private static BoxController Instance;
    public static BoxController GetInstance()
    {
        return Instance;
    }

    [SerializeField] private GameObject box;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 finalPoint;
    [SerializeField] private bool canMove = false;

    public Action<BOX_STATE> OnBoxStateChange;
    private BOX_STATE boxState = BOX_STATE.NORMAL;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveBox();
        }
    }

    void RespawnBox()
    {
        box.transform.position = spawnPoint;
        box.SetActive(true);
        canMove = true;
    }

    void MoveBox()
    {
        float distance = spawnPoint.y - finalPoint.y;
        float destiny = distance / (60 * 1f);

        box.transform.position -= new Vector3(0, destiny, 0);

        if (box.transform.position.y <= finalPoint.y)
        {
            box.transform.position = finalPoint;
            canMove = false;
            ChangeBoxState(BOX_STATE.NORMAL);
        }
    }

    public void ChangeBoxState(BOX_STATE _newBoxState)
    {
        boxState = _newBoxState;

        if (OnBoxStateChange != null)
        {
            OnBoxStateChange.Invoke(boxState);
        }

        if (boxState == BOX_STATE.SPAWN)
        {
            RespawnBox();
        }
    }
}

public enum BOX_STATE
{
    SPAWN,
    NORMAL,
    FALLING,
}
