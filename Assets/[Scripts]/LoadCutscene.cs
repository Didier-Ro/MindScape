using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCutscene : MonoBehaviour
{
    public float changeTime;
    public string sceneName;

    public void Update()
    {
        changeTime -= Time.deltaTime;
        if(changeTime <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
