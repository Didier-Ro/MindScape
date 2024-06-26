using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    public GameObject loadingScreen;
    public TextMeshProUGUI progressText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress
            progressText.text = "Loading: " + (progress * 100).ToString("0") + "%";
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}
