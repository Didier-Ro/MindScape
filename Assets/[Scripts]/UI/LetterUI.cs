using UnityEngine;

public class LetterUI : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
