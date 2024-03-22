using UnityEngine;

public class Sign : MonoBehaviour, Istepable
{
    [SerializeField] private Dialog dialog;
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
    }
}
