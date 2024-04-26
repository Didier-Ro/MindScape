using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ikillable
{
   public void Hit(Transform player);

    public void UnHit(Transform player);
}
