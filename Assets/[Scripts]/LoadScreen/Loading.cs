using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    enum LOAD_PART
    {
        LOADING_SCENE,
        UNLOAD_SCENE,
        LOAD_NEXT,
        NONE
    }

    LOAD_PART nextLoad = LOAD_PART.NONE;

    public string sceneToLoad;
    public int sceneToLoadNum = -1;
    const string loadingSceneName = "LoadingScreen";

    public Animation loadScreen;

    public event Action OnStartChangingScene;

    int currentScene;

    private void Awake()
    {
        nextLoad = LOAD_PART.NONE;  
    }

    public void ChangeToLoad()
    {
        Debug.Log("1-----Start Loading");
        DontDestroyOnLoad(gameObject);
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadScreen.gameObject.SetActive(true);
        loadScreen.Play("LoadingAnim");
    }

    public void OnLoadingScreenSet()
    {
        Debug.Log("3------Start Loading");
        nextLoad = LOAD_PART.LOADING_SCENE;
        if( OnStartChangingScene != null )
        {
            OnStartChangingScene.Invoke();
        }
        SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive); //Obtenemos load screen
    }
    void OnSceneLoaded(Scene _scene, LoadSceneMode _load) //Llamada automatica por SceneManager
    {
        Debug.Log("4--------Loading scene done");
        StartCoroutine(WaitOnLevelOLoaded());
    }

    IEnumerator WaitOnLevelOLoaded()
    {
        if (nextLoad == LOAD_PART.LOADING_SCENE) //Escena 1 y loading screen al mismo tiempo
        {
            nextLoad = LOAD_PART.UNLOAD_SCENE;
            while (SceneManager.sceneCount < 2)
            {
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("5--------start unloading scene 1");
            SceneManager.UnloadSceneAsync(currentScene); //quitamos escena 1 y dejamos loading screen
        }
        else if (nextLoad == LOAD_PART.LOAD_NEXT)
        {
            Debug.Log("7--------Unload scene 2 done");
            nextLoad = LOAD_PART.NONE;
            while (SceneManager.sceneCount < 2)
            {
                yield return new WaitForEndOfFrame();
            }
            loadScreen.Play("LoadingAnimOut");
            Debug.Log("8--------Remove loading screen");
            SceneManager.UnloadSceneAsync(loadingSceneName); //quitamos loading screen
        }
    }

    void OnSceneUnloaded(Scene _scene) //Llamada automatica por SceneManager
    {
        if (nextLoad != LOAD_PART.UNLOAD_SCENE)
            return;

        Debug.Log("6--------Unload scene 1 done, load scene 2");
        nextLoad = LOAD_PART.LOAD_NEXT;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        if (sceneToLoadNum >= 0)
        {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive); //cargamos escena 2
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive); //cargamos escena 2
        }
    }

    public void OnLoadingScreenOff()
    {
        Debug.Log("10--------End of process");
        Destroy(gameObject);
    }
}
