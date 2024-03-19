using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour
{
    public TabGroup tabGroup = default;

    public Image background;

    private void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }
}
