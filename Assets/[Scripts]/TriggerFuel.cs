using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TriggerFuel : MonoBehaviour
{
    public GameObject player;
    public GameObject playerSpotLight;
    public GameObject light2D;
    public GameObject lantern;
    public GameObject timeline;
    public GameObject dashOn1;
    public GameObject dashOn2;
    public GameObject dashOn3;
    public GameObject dashOff1;
    public GameObject dashOff2;
    public GameObject dashOff3;
    public GameObject fadeIn;
    public Light2D globalLight;
    public Flashlight playerFlashlight;
    public int conditionID;

    private bool isCoroutineRunning = false;
    private bool isPaused = false;

    private void Start()
    {
        SubscribeToGameManagerGameState();
    }

    private void OnDestroy()
    {
        player.SetActive(true);
        Flashlight.GetInstance().isInInitialRoom = false;
        playerSpotLight.SetActive(true);
        light2D.SetActive(true);
        playerFlashlight.enabled = true;
        globalLight.intensity = 0.1f;
        lantern.SetActive(true);
        dashOn1.SetActive(true);
        dashOn2.SetActive(true);
        dashOn3.SetActive(true);
        dashOff1.SetActive(true);
        dashOff2.SetActive(true);
        dashOff3.SetActive(true);
        fadeIn.SetActive(false);
        Destroy(timeline);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isCoroutineRunning)
            {
                StartCoroutine(LightIntensityFlicker());
            }
            Flashlight.GetInstance().isInInitialRoom = false;
            playerSpotLight.SetActive(true);
            light2D.SetActive(true);
            playerFlashlight.enabled = true;
            lantern.SetActive(true);
            dashOn1.SetActive(true);
            dashOn2.SetActive(true);
            dashOn3.SetActive(true);
            dashOff1.SetActive(true);
            dashOff2.SetActive(true);
            dashOff3.SetActive(true);
            fadeIn.SetActive(false);
        }
    }

    IEnumerator LightIntensityFlicker()
    {
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.DASHING);
        GameManager.GetInstance().ChangeGameState(GAME_STATE.READING);
        if (!isPaused) 
        {
            isCoroutineRunning = true;
        }

        float flickerDuration = 3f;
        float flickerStartTime = Time.time;
        float flickerEndTime = flickerStartTime + flickerDuration;

        while (Time.time < flickerEndTime)
        {
            // Cambia entre 1 y 0.1 cada 5 frames
            if (Time.frameCount % 20 == 0)
            {
                globalLight.intensity = (globalLight.intensity == 1f) ? 0.1f : 1f;
            }
            yield return null;
        }

        // Aseg�rate de que la intensidad sea 0.1 al final
        globalLight.intensity = 0.1f;
        isCoroutineRunning = false;

        gameObject.SetActive(false);
        PlayerStates.GetInstance().ChangePlayerState(PLAYER_STATES.PLAY);
        GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
    }

    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
            OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
        }
    }

    private void OnGameStateChange(GAME_STATE _newGameState)
    {
        isPaused = _newGameState == GAME_STATE.PAUSE;
    }
}
