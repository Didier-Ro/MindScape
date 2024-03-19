using UnityEngine;

public class Letter : MonoBehaviour, Istepable
{
    [SerializeField] private LetterScriptable dialog;
    
    public void Activate()
    {
        Debug.Log("Reading");
        StartCoroutine(DialogManager.GetInstance().ShowDialog(dialog));
        GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
    }

    public void Deactivate()
    {
        Debug.Log("Exit");
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        Destroy(gameObject);
    }
    
}
