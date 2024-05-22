using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCutscene : MonoBehaviour
{
    public string sceneName;

    public void ChangeToGameplay()
    {
        SceneManager.LoadScene(sceneName);
    }

}
