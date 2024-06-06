using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class DebugScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sceneName;

    private void Start()
    {
        Scene _scene = SceneManager.GetActiveScene();
        sceneName.text = _scene.name;
    }
} 
