using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Feet")
        {
            PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.FALL);
        }
    }
}
