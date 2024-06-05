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
        Debug.Log("2------Start animation");
        loading.OnLoadingScreenSet();
    }  
    
    public void LoadingScreenOut()
    {
        Debug.Log("9--------End of animation");
        loading.OnLoadingScreenOff();
        Destroy(gameObject);
    }
}
