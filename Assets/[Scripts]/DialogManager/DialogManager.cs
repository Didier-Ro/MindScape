using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class DialogManager : MonoBehaviour
{
    public event Action OnCloseDialog;
    [SerializeField] private GameObject dialogBox = default;
    [SerializeField] private TextMeshProUGUI dialogText = default;
    [SerializeField] private int lettersPerSecond = default;
    private bool isTyping = false;
    private Dialog dialog = default;
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
//        Debug.Log(this.dialog.Lines);
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(this.dialog.Lines[0]));
    }

    
    public void HandleUpdate()
    {
        if (InputManager.GetInstance().NextInput() && !isTyping)
        {
            Debug.Log("Se presionó");
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }
    
    public IEnumerator TypeDialog(string _line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in _line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        isTyping = false;
    }
}
