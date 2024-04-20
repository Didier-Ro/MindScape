using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBoxDetectorManager : MonoBehaviour
{
    [SerializeField] private List<BoxCollider2D> overlapDetectors = new List<BoxCollider2D>();
    [SerializeField] private Vector3 spawnVector;
    private OverlapBoxDetector overlapBoxDetector;

    public Vector3 FindVoidPlace()
    {
        for (int i = 0; i < overlapDetectors.Count; i++)
        {
            overlapBoxDetector = overlapDetectors[i].GetComponent<OverlapBoxDetector>();
            if (!overlapBoxDetector.IsBoxInPlace())
            {
                spawnVector = overlapDetectors[i].transform.position;
                break;
            }
            else
            {
                continue;
            }
        }
        return spawnVector;
    }
}
