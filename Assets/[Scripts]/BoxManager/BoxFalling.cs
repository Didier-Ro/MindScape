using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFalling : MonoBehaviour
{
    [SerializeField] bool isFalling;
    [SerializeField] Transform boxtransform;
    void Start()
    {
        SubscribeToBoxController();
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            Falling();
        }
    }

    void SubscribeToBoxController()
    {
        if (BoxController.GetInstance() != null)
        {
            BoxController.GetInstance().OnBoxStateChange += OnBoxStateChanged;
        }
    }

    void OnBoxStateChanged(BOX_STATE _newBoxState)
    {
        if (_newBoxState == BOX_STATE.FALLING)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    void Falling()
    {
        float totalSize = 1 / (60 * 1);

        boxtransform.localScale -= new Vector3(totalSize, totalSize, 0);

        /*if (transform.localScale.y <= 0 && transform.localScale.x <= 0)
        {
            transform.localScale = new Vector3(0,0,0);
            BoxController.GetInstance().ChangeBoxState(BOX_STATE.SPAWN);
        }*/
    }
}
