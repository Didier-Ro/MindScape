using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject[] Obstacles;
    public GameObject[] Boxes;
    // Start is called before the first frame update

    public bool ReadytoMove;
    void Start()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        Boxes = GameObject.FindGameObjectsWithTag("Boxes");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveinput.Normalize();

        if (moveinput.sqrMagnitude > 0.5)
        {
            if(ReadytoMove)
            {
                ReadytoMove = false;
                Move(moveinput);
            }
        }
        else
        {
            ReadytoMove = true;
        }
            
    }
    public bool Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x)<0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction.Normalize();

        if(Blocked(transform.position,direction))
            {
            return false;
        }
        else
        {
            transform.Translate(direction);
            return true;
        }
            
    }

    public bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPosition= new Vector2(position.x, position.y)+direction;

        foreach(var obj in Obstacles)
        {
            if(obj.transform.position.x==newPosition.x && obj.transform.position.y==newPosition.y)
            {
                return true;
            }
        }

        foreach(var objToPush in Boxes)
        {
            if(objToPush.transform.position.x==newPosition.x && objToPush.transform.position.y==newPosition.y) 
            {
                Push objpush = objToPush.GetComponent<Push>();
                if(objpush && objpush.Move(direction))
                { 
                    return false; 
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
}
