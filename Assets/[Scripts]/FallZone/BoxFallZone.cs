using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFallZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            collision.GetComponentInParent<BoxFalling>().BoxInZone();
        }
    }
}
