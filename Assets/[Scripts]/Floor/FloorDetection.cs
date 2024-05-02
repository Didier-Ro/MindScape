using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetection : MonoBehaviour
{
    private List<BoxCollider2D> boxColliders = new List<BoxCollider2D>(); 
    private GameObject floor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Floor"))
        {
            boxColliders.Add(collision.GetComponent<BoxCollider2D>());
            floor = boxColliders[boxColliders.Count - 1].gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("Floor"))
        {
            if(boxColliders.Count > 2)
            {
                boxColliders.RemoveAt(0);
                floor = boxColliders[boxColliders.Count - 1].gameObject;
            }
        }
    }

    public void ActivateFloorSound()
    {
        
        floor.GetComponent<Fstepable>().FActivate();
    }
}
