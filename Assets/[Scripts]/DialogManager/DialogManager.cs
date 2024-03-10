using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBox = default;
    [SerializeField] private TextMeshProUGUI _dialogText = default;
    [SerializeField] private int letterspPerSecond = default;
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

    private void Update()
    {
        Debug.Log("La linea se encuentra en: "+ currentLine);
      
        
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(dialog.Lines);
        this.dialog = dialog;
        _dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
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
                _dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }
    
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        _dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            _dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterspPerSecond);
        }

        isTyping = false;
    }
}
