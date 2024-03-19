using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    [SerializeField] private GameObject floor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Floor"))
        {
            floor = collision.gameObject; 
        }
    }

    public void ActivateFloorSound()
    {
        floor.GetComponent<Fstepable>().FActivate();
    }
}
