using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxDetector : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private bool isBoxInPlace;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            Debug.Log("Caja en lugar de Spawn");
            isBoxInPlace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            Debug.Log("No hay caja en medio");
            isBoxInPlace = false;
        }
    }

    public bool IsBoxInPlace()
    {
        return isBoxInPlace;
    }
}
