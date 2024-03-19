using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons = new List<TabButton>();

    public void Subscribe(TabButton button)
    {
       tabButtons.Add(button);
    }

    public void OnTabExit(TabButton button)
    {
        
    }
    public void OnTabSelected(TabButton button)
    {
        
    }
    public void OnTabEnter(TabButton button)
    {
        
    }


}
