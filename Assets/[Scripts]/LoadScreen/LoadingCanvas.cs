using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    public Loading loading;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadingScreenIn()
    {
        loading.OnLoadingScreenSet();
    }  
    
    public void LoadingScreenOut()
    {
        loading.OnLoadingScreenOff();
        Destroy(gameObject);
    }
}
