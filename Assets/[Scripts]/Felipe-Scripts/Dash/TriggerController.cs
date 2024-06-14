using TMPro;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    public GameObject dashText;
    public GameObject jumpText;

    private bool isActive = true;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive && !activated)
        {
            activated = true;
            if (gameObject.name == "DashTrigger")
            {
                dashText.SetActive(true);
            }
            else if (gameObject.name == "JumpTrigger")
            {
                jumpText.SetActive(true);
            }

            AudioManager.GetInstance().SetSound(SOUND_TYPE.Tutorial_corto);

            FindObjectOfType<DashController>().SetCurrentTrigger(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dashText.SetActive(false);
            jumpText.SetActive(false);
            activated = false;
        }
    }

    public void DisableTrigger()
    {
        isActive = false;
    }
}