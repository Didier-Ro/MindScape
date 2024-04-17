using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 finalPoint;
    [SerializeField] private bool canMove = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
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
        }
    }
}
