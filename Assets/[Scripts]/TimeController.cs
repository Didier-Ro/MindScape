using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject tutorialText;

    public float timeScale;
    private float startTimeScale;
    private float startFixedDeltaTime;

    private bool enteredEnemyZone = false;

    void Start()
    {
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (enteredEnemyZone)
        {
            StartSlowMotion();
            tutorialText.SetActive(true);
            if (InputManager.GetInstance().FlashligthInput())
            {
                enteredEnemyZone = false;
                StopSlowMotion();
                this.enabled = false;
                tutorialText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enteredEnemyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enteredEnemyZone = false;
        }
    }

    public void StartSlowMotion()
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * timeScale;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }
}
