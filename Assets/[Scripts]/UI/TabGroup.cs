using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons = new List<TabButton>();
    public TabButton selectedTab = default;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    private int counter = 0;
    

    public void Subscribe(TabButton button)
    {
        tabButtons.Add(button);
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        ResetTabs();
        selectedTab = button;
        button.background.sprite = tabSelected;
        button.ActivateCanvas();

    }
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        button.background.sprite = tabHover;
    }

    public void ResetAll()
    {
        counter = 0;
        OnTabSelected(tabButtons[counter]);
    }

    private void ArrayCheck(int value)
    {
        if (value == 1 && counter+1 == tabButtons.Count)
        {
            counter = 0;
        }
        else if(value == -1 && counter == 0)
        {
            counter = tabButtons.Count - 1;
        }
        else
        {
            counter += value;
        }
    }

    private void Update()
    {
        if (InputManager.GetInstance().NextUIInput())
        {
            ArrayCheck(1);
            OnTabSelected(tabButtons[counter]);
        } 
        else if (InputManager.GetInstance().PreviousUIInput())
        {
            ArrayCheck(-1);
            OnTabSelected(tabButtons[counter]);
        }
      
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            button.DeactivateCanvas();
            button.background.sprite = tabIdle;
        }
    }


}