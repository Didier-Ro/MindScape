using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : InteractableObjects
{
    public override void InteractionResponse()
    {
        base.InteractionResponse();
        Debug.Log("Leyendo");
    }

}
