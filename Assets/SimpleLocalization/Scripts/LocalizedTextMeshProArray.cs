using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts
{
    /// <summary>
    /// Localize text component.
    /// </summary>
public class LocalizedTextMeshProArray : MonoBehaviour
{
    public List<String> texts = new List<String>();
    public List<String> localizationKey = new List<string>();
    public void OnEnable()
    {
        Localize();
        LocalizationManager.OnLocalizationChanged += Localize;
    }

    public void OnDestroy()
    {
        LocalizationManager.OnLocalizationChanged -= Localize;
    }
    private void Localize()
    {
        foreach (var key in localizationKey)
        {
           texts.Add(LocalizationManager.Localize(key));
        }
    }
    
} 
}
