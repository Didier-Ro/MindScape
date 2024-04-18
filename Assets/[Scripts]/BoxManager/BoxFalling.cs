using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFalling : MonoBehaviour
{
    [SerializeField] bool isFalling;
    [SerializeField] Transform boxtransform;

    private float size;
    private float totalSize;

    void Start()
    {
        SubscribeToBoxController();
        size = 1 - 0;
        totalSize = size / (60 * 1);
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
        boxtransform.localScale -= new Vector3(totalSize, totalSize,0);

        if (boxtransform.localScale.y <= 0 || boxtransform.localScale.x <= 0)
        {
            boxtransform.localScale = new Vector3(0,0,0);
            isFalling = false;
            BoxController.GetInstance().ChangeBoxState(BOX_STATE.SPAWN);
        }
    }
}
