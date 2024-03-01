using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    private GameObject[] Obstacles;
    private GameObject[] Boxes;
    // Start is called before the first frame update
    void Start()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        Boxes = GameObject.FindGameObjectsWithTag("Boxes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Move(Vector2 direction)
    {
        if(ObjtoBlock(transform.position,direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            return true;
        }
    }
    public bool ObjtoBlock(Vector3 position, Vector2 direction)
    {
        Vector2 newPosition = new Vector2(position.x, position.y) + direction;

        foreach (var obj in Obstacles)
        {
            if (obj.transform.position.x == newPosition.x && obj.transform.position.y == newPosition.y)
            {
                return true;
            }
        }

        foreach (var obj in Boxes)
        {
            if (obj.transform.position.x == newPosition.x && obj.transform.position.y == newPosition.y)
            {
                return true;
            }
        }
        return false;
    }
}
