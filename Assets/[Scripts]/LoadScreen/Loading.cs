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
        DontDestroyOnLoad(gameObject);
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadScreen.gameObject.SetActive(true);
        loadScreen.Play("LoadingAnim");
    }

    public void OnLoadingScreenSet()
    {
        nextLoad = LOAD_PART.LOADING_SCENE;
        if( OnStartChangingScene != null )
        {
            OnStartChangingScene.Invoke();
        }
        SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive); //Obtenemos load screen
    }
    void OnSceneLoaded(Scene _scene, LoadSceneMode _load) //Llamada automatica por SceneManager
    {
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

            SceneManager.UnloadSceneAsync(currentScene); //quitamos escena 1 y dejamos loading screen
        }
        else if (nextLoad == LOAD_PART.LOAD_NEXT)
        {
            nextLoad = LOAD_PART.NONE;
            while (SceneManager.sceneCount < 2)
            {
                yield return new WaitForEndOfFrame();
            }
            loadScreen.Play("LoadingAnimOut");
            SceneManager.UnloadSceneAsync(loadingSceneName); //quitamos loading screen
        }
    }

    void OnSceneUnloaded(Scene _scene) //Llamada automatica por SceneManager
    {
        if (nextLoad != LOAD_PART.UNLOAD_SCENE)
            return;
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
        Destroy(gameObject);
    }
}
