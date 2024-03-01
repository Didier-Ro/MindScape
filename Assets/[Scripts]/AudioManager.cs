using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private AudioSource musicSource, SfxSource = default;

    #region SingeTone
    public AudioManager GetInstance()
    {
        return _instance;
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    #endregion

    #region SubscribeToGameManager
    private void SubscribeToGameManagerGameState()//Subscribe to Game Manager to receive Game State notifications when it changes
    {
        GameManager.GetInstance().OnGameStateChange += OnGameStateChange;
        OnGameStateChange(GameManager.GetInstance().GetCurrentGameState());
    }
    private void OnGameStateChange(GAME_STATE _newGameState)//Analyze the Game State type and shows a different UI
    {
        switch (_newGameState)
        {
            case GAME_STATE.PAUSE:
                StopSound();
                break;
            case GAME_STATE.EXPLORIMG:
               ResumeSound();
                break;
            case GAME_STATE.READING:
               
                break;
            case GAME_STATE.DEAD:
               
                break;
        }   
    }
    #endregion

    private void ResumeSound()
    {
        musicSource.UnPause();
        SfxSource.UnPause();
    }

    private void StopSound()
    {
        musicSource.Pause();
        SfxSource.Pause();
    }
    private void Start()
    {
       SubscribeToGameManagerGameState();
    }

    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }
}
