using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        LoadingManager.instance.LoadScene(sceneToLoad);
    }
}
