using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjective : MonoBehaviour
{
    [SerializeField] private GameObject objectiveGameObject;
    [SerializeField] private string ObjectToDetect;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ObjectToDetect))
        {
            CameraManager.instance.ChangeTargetCamera(objectiveGameObject);
        }
    }
}
