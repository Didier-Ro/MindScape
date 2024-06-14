using UnityEngine;

public class DisableMouseCursor : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void EnableMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void DisableMouseCursorAgain()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}