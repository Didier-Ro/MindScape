using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    #region Singletone
    private static Flashlight Instance;
    public static Flashlight GetInstance()
    {
        return Instance;
    }
    #endregion

    public Slider slider;
    public float currentSliderValue = 100f;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private Light2D wallFlashLight;
    [SerializeField] private float maxPointLightInnerAngle = 360;
    [SerializeField] private float maxPointLightOuterAngle = 360;
    [SerializeField] private float minPointLightInnerAngle = 0;
    [SerializeField] private float minPointLightOuterAngle = 70;
    [SerializeField] private float maxPointLightOuterRadius = 3;
    [SerializeField] private float minPointLightOuterRadius = 3;
    [SerializeField] private float maxLightIntensity = 1;
    [SerializeField] private float minLightIntensity = 0.7f;
    [SerializeField] private float intensityTimeSpeed;
    [SerializeField] private float lightInnerAngleTimeSpeed;
    [SerializeField] private float lightOuterAngleTimeSpeed;
    [SerializeField] private float energy = 100f; // Initial energy value
    private bool isExPloration = false;
    public bool isInInitialRoom = true;

    private float reductionSpeed;
    [SerializeField] private GameObject cameraView = default;
    [SerializeField] private float angleRange;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float radius;
    private bool _canSeeTarget = false;


    [Header("RotateLight")]
    private Vector2 lastMousePosition = Vector2.zero;
    [SerializeField] private Transform flashLightTransform;
    [SerializeField] private float rotationSpeed = default;
    [SerializeField] private Transform wallFlashLightTransform;
    [SerializeField] private Camera camera = default;
    private float offsetAngle = 270;
    private float lastAngle = 0;

    private GameObject activeCircleParticles;
    private GameObject activeConcentratedParticles;

    [SerializeField] private SoundLibrary soundLibrary;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private FlashlightEnergy flashlightEnergy;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GameManager.GetInstance().GetFlashlightReferecen(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToGameManagerGameState();
        InitializeFlashlight();
        LightSetUp();
        angleRange = minPointLightOuterAngle / 2;
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = GetComponentInChildren<AudioSource>();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isExPloration)
            return;
        HandleInput();
    }

    public void SetFlashlightEnergy()
    {
        currentSliderValue = GameManager.GetInstance().GetFlashligthEnergy();
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
        isExPloration = _newGameState == GAME_STATE.EXPLORATION;
    }

    // Initialize the flashlight settings
    private void InitializeFlashlight()
    {
        CircleLight();
        SetFlashlightEnergy();
    }

    // Handle input to toggle flashlight mode
    private void HandleInput()
    {
        if (!GameManager.GetInstance().GetFlashing())
        {
            CircleLight();
        }
        else
        {
            ConcentrateLight();
            RotateLight();
            EnemyLanternCheck();
        }
    }

    private void RotateLight()
    {
        float angle = 0;
        if (Gamepad.current != null)
        {
            Vector2 inputLight = InputManager.GetInstance().MoveLightInput();
            angle = Mathf.Atan2(inputLight.y, inputLight.x) * Mathf.Rad2Deg;
            if (inputLight == Vector2.zero) return;
        }
        else if (Mouse.current != null)
        {
            Vector2 inputLight = camera.ScreenToWorldPoint(InputManager.GetInstance().MoveLightInput());
            Vector2 direction = (inputLight - (Vector2)transform.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        angle += offsetAngle;
        //float difference = lastAngle - angle;
        flashLightTransform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, lastAngle), Quaternion.Euler(0, 0, angle), rotationSpeed);
        wallFlashLightTransform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, lastAngle), Quaternion.Euler(0, 0, angle), rotationSpeed);
        /* if (cameraView != null)
         {
             cameraView.transform.rotation = Quaternion.Slerp(Quaternion.Euler(0,0, lastAngle), Quaternion.Euler(0, 0, angle), rotationSpeed);
             CameraManager.instance.ChangeCameraToAnObject(cameraView);
         }*/
        if (activeConcentratedParticles != null)
        {
            activeConcentratedParticles.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        lastAngle = flashLightTransform.rotation.z;
    }

    private float ReduceErrorZero(float value)
    {
        if (Mathf.Approximately(value, 0))
        {
            value = Mathf.Epsilon;
        }
        return value;
    }

    private void EnemyLanternCheck()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
        if (currentSliderValue <= 0)
        {
            foreach (Collider2D collider2D in rangeCheck)
            {
                if (collider2D.GetComponent<Ikillable>() != null)
                {
                    collider2D.GetComponent<Ikillable>().UnHit(transform);
                }
            }
            return;
        }
        if (rangeCheck.Length == 0) return;
        foreach (Collider2D col in rangeCheck)
        {
            Vector2 direction = col.transform.position - transform.position;
            direction.x = ReduceErrorZero(direction.x);
            direction.y = ReduceErrorZero(direction.y);
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = Mathf.Repeat(angleRadians * Mathf.Rad2Deg, 360);
            float lightAngle = Mathf.Repeat(flashLightTransform.rotation.eulerAngles.z - offsetAngle, 360);
            lightAngle = Mathf.Repeat(lightAngle, 360);
            float upperLimitLightAngle = Mathf.Repeat(lightAngle + angleRange, 360);
            float lowerLimitLightAngle = Mathf.Repeat(lightAngle - angleRange, 360);
            if (lowerLimitLightAngle < upperLimitLightAngle)
            {
                if (angleDegrees > lowerLimitLightAngle && upperLimitLightAngle > angleDegrees)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, col.transform.position); //Minium distance to see the target
                    _canSeeTarget = !Physics2D.Raycast(transform.position, direction, distanceToTarget, obstructionMask);
                    if (!_canSeeTarget) return;
                    col.GetComponent<Ikillable>().Hit(transform);
                }
                else if (_canSeeTarget)
                {
                    _canSeeTarget = false;
                    col.GetComponent<Ikillable>().UnHit(transform);
                }
                else
                {
                    col.GetComponent<Ikillable>().UnHit(transform);
                }
            }
            else
            {
                bool secondSegment = angleDegrees < upperLimitLightAngle && lowerLimitLightAngle >= 360 - angleRange * 2;
                bool firstSegment = upperLimitLightAngle <= angleRange * 2 && angleDegrees >= lowerLimitLightAngle;
                if (secondSegment || firstSegment)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, col.transform.position); //Minium distance to see the target
                    _canSeeTarget = !Physics2D.Raycast(transform.position, direction, distanceToTarget, obstructionMask);
                    if (!_canSeeTarget) return;
                    col.GetComponent<Ikillable>().Hit(transform);
                }
                else if (_canSeeTarget)
                {
                    _canSeeTarget = false;
                    col.GetComponent<Ikillable>().UnHit(transform);
                }
                else
                {
                    col.GetComponent<Ikillable>().UnHit(transform);
                }
            }
        }
    }


    // Set flashlight settings for circle light mode
    private void LightSetUp()
    {
        float time = 0.3f;
        int frame = 60;

        float totalIntensityValue = maxLightIntensity - minLightIntensity;
        float totalLightInnerAngle = maxPointLightInnerAngle - minPointLightInnerAngle;
        float totalLightOuterAngle = maxPointLightOuterAngle - minPointLightOuterAngle;

        intensityTimeSpeed = totalIntensityValue / (frame * time);
        lightInnerAngleTimeSpeed = totalLightInnerAngle / (frame * time);
        lightOuterAngleTimeSpeed = totalLightOuterAngle / (frame * time);
    }

    private void CircleLight()
    {
        if (isInInitialRoom)
        {
            ReduceSliderValue(0.0f);
        }
        else
        {
            ReduceSliderValue(0.01f);
        }
        StopConcentratedSound();
        if (activeCircleParticles == null)
        {
            activeCircleParticles = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.ChispasCirculo, transform.position, new Vector3(0, -90, 0));
            if (activeCircleParticles != null)
            {
                var particleSystem = activeCircleParticles.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }
        if (activeConcentratedParticles != null)
        {
            activeConcentratedParticles.SetActive(false);
            activeConcentratedParticles = null;
        }

        if (activeCircleParticles != null)
        {
            activeCircleParticles.transform.position = transform.position;
            activeCircleParticles.transform.rotation = Quaternion.Euler(0, -90, 0);
            activeCircleParticles.SetActive(true);

            var particleSystem = activeCircleParticles.GetComponent<ParticleSystem>();
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        flashlight.intensity -= intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 6.71f;
        flashlight.pointLightInnerRadius = 2.6f;
        wallFlashLight.pointLightOuterRadius = 6.71f;
        wallFlashLight.pointLightInnerRadius = 2.6f;
        flashlight.pointLightInnerAngle += lightInnerAngleTimeSpeed;
        wallFlashLight.pointLightInnerAngle += lightInnerAngleTimeSpeed;
        flashlight.pointLightOuterAngle += lightOuterAngleTimeSpeed;
        wallFlashLight.pointLightOuterAngle += lightOuterAngleTimeSpeed;

        if (flashlight.intensity <= minLightIntensity)
        {
            flashlight.intensity = minLightIntensity;
            wallFlashLight.intensity = minLightIntensity;
        }
        if (flashlight.pointLightInnerAngle >= maxPointLightInnerAngle)
        {
            flashlight.pointLightInnerAngle = maxPointLightInnerAngle;
            wallFlashLight.pointLightInnerAngle = maxPointLightInnerAngle;
        }

        if (flashlight.pointLightOuterAngle >= maxPointLightOuterAngle)
        {
            flashlight.pointLightOuterAngle = maxPointLightOuterAngle;
            wallFlashLight.pointLightOuterAngle = maxPointLightOuterAngle;
        }
        if (activeConcentratedParticles != null)
        {
            activeConcentratedParticles.SetActive(false);
            activeConcentratedParticles = null;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

    }
    private void StopConcentratedSound()
    {
        if (audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }

    // Set flashlight settings for concentrated light mode
    private void ConcentrateLight()
    {
        if (isInInitialRoom)
        {
            ReduceSliderValue(0.0f);
        }
        else
        {
            ReduceSliderValue(0.1f);
        }
        PlayConcentratedSound();
        if (activeConcentratedParticles == null)
        {
            activeConcentratedParticles = PoolManager.GetInstance().GetPooledObject(OBJECT_TYPE.Linternacerradaconluz, transform.position, transform.rotation.eulerAngles);
            if (activeConcentratedParticles != null)
            {
                activeConcentratedParticles.transform.rotation = Quaternion.Euler(0, 0, flashLightTransform.rotation.eulerAngles.z);
                var particleSystem = activeConcentratedParticles.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }
        if (activeCircleParticles != null)
        {
            activeCircleParticles.SetActive(false);
            activeCircleParticles = null;
        }

        if (activeConcentratedParticles != null)
        {
            activeConcentratedParticles.transform.position = transform.position;
            activeConcentratedParticles.transform.rotation = Quaternion.Euler(0, 0, flashLightTransform.rotation.eulerAngles.z);
            activeConcentratedParticles.SetActive(true);

            var particleSystem = activeConcentratedParticles.GetComponent<ParticleSystem>();
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        flashlight.intensity += intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 15;
        flashlight.pointLightInnerRadius = 6;
        wallFlashLight.pointLightOuterRadius = 15;
        wallFlashLight.pointLightInnerRadius = 6;
        flashlight.pointLightInnerAngle -= 2 * lightInnerAngleTimeSpeed;
        flashlight.pointLightOuterAngle -= 2 * lightOuterAngleTimeSpeed;
        wallFlashLight.pointLightInnerAngle -= 2 * lightInnerAngleTimeSpeed;
        wallFlashLight.pointLightOuterAngle -= 2 * lightOuterAngleTimeSpeed;

        if (flashlight.intensity >= maxLightIntensity)
        {
            flashlight.intensity = maxLightIntensity;
            wallFlashLight.intensity = maxLightIntensity;
        }
        if (flashlight.pointLightInnerAngle <= minPointLightInnerAngle)
        {
            flashlight.pointLightInnerAngle = minPointLightInnerAngle;
            wallFlashLight.pointLightInnerAngle = minPointLightInnerAngle;
        }

        if (flashlight.pointLightOuterAngle <= minPointLightOuterAngle)
        {
            flashlight.pointLightOuterAngle = minPointLightOuterAngle;
            wallFlashLight.pointLightOuterAngle = minPointLightOuterAngle;
        }
    }
    /*private void OnEnable()
    {
        if (!audioSource.isPlaying)
        {
            AudioClip soundClip = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.LAMP_ON);
            if (soundClip != null)
            {
                audioSource.clip = soundClip;
                audioSource.volume = 0.1f;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }*/
    private void PlayConcentratedSound()
    {
        if (!audioSource.isPlaying)
        {
            AudioClip soundClip = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.RAYO_DE_LUZ_RELOADED);
            if (soundClip != null)
            {
                audioSource.clip = soundClip;
                audioSource.volume = 0.1f;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    public void ReduceSliderValue(float _reductionSpeed)
    {
        reductionSpeed = _reductionSpeed;
        currentSliderValue -= reductionSpeed;
        slider.value = currentSliderValue;

        if (currentSliderValue <= 0)
        {
            // Detener el sonido concentrado si está activo
            StopConcentratedSound();

            // Reproducir el sonido de apagado de la linterna si la linterna está encendida
            if (flashlight.gameObject.activeSelf)
            {
                if (audioSource != null && soundLibrary != null && !audioSource.isPlaying)
                {
                    AudioClip lampOffSound = soundLibrary.GetRandomSoundFromType(SOUND_TYPE.LAMP_OFF);
                    if (lampOffSound != null)
                    {
                        audioSource.PlayOneShot(lampOffSound);
                    }
                }
            }

            // Apagar la linterna
            if (flashlight.gameObject.activeSelf)
            {
                flashlight.gameObject.SetActive(false);
                wallFlashLight.gameObject.SetActive(false);
                currentSliderValue = 0;
            }
        }
        else
        {
            // Si la linterna no está apagada, asegurarse de que esté encendida
            flashlight.gameObject.SetActive(true);
            wallFlashLight.gameObject.SetActive(true);
        }
    }

    // Function to get current energy level
    public float GetEnergy()
    {
        return currentSliderValue;
    }
}