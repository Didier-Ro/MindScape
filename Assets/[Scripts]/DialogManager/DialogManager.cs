using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox = default;
    [SerializeField] private TextMeshProUGUI dialogText = default;
    [SerializeField] private int lettersPerSecond = default;
    private bool isTyping = false;

   
    public event Action OnCloseDialog;
    private Dialog dialog;
    private int currentLine = 0;
    
    
    #region Singletone
    private static DialogManager Instance;
    public static DialogManager GetInstance() 
    { 
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    #endregion

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public IEnumerator TypeDialog(string line)
    {
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    public void HandleUpdate()
    {
        if (isTyping && InputManager.GetInstance().NextInput())
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
        }
    }
}
