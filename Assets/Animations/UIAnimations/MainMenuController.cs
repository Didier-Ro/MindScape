using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public Animation animation;

   
    public void EnterAnimation()
    {
        animation.Play("MAIN_MENU_ANIM");
    }

    public void ExitAnim()
    {
        
    }
}
