using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Feet")
        {
            if(PlayerStates.GetInstance().GetCurrentPlayerState() != PLAYER_STATES.DASHING)
                PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.FALL);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Feet")
        {
            if (PlayerStates.GetInstance().GetCurrentPlayerState() != PLAYER_STATES.DASHING)
                PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.FALL);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Feet")
        {
            if (PlayerStates.GetInstance().GetCurrentPlayerState() != PLAYER_STATES.DASHING)
            {
                PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
            }
        }
    }
}
