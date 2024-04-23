using System;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        LoadingManager.instance.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            LoadingManager.instance.LoadScene(sceneToLoad);
        }
    }
}
