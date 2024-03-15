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
    private bool flashing = false;
    
    private float reductionSpeed;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeFlashlight();
        LightSetUp();
        angleRange = minPointLightInnerAngle / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleInput();
    }

    // Initialize the flashlight settings
    private void InitializeFlashlight()
    {
        CircleLight();
        energy = slider.maxValue;
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
        }
        else if (Mouse.current != null)
        {
            Vector2 inputLight = camera.ScreenToWorldPoint(InputManager.GetInstance().MoveLightInput());
            Vector2 direction = (inputLight - (Vector2)transform.position).normalized;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        angle += offsetAngle;
        flashLightTransform.rotation = Quaternion.Slerp(flashLightTransform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed);
        wallFlashLightTransform.rotation  = Quaternion.Slerp(wallFlashLightTransform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed);
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
        if (currentSliderValue <= 0) return;
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
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
          //Debug.Log(angleDegrees + " > " + lowerLimitLightAngle + " && " + upperLimitLightAngle +" < " + angleDegrees);
            if (angleDegrees > lowerLimitLightAngle && upperLimitLightAngle > angleDegrees)
            {
                float distanceToTarget = Vector2.Distance(transform.position, col.transform.position); //Minium distance to see the target
                _canSeeTarget = !Physics2D.Raycast(transform.position, direction, distanceToTarget, obstructionMask);
                if (!_canSeeTarget) return;
                 col.GetComponent<Ikillable>().Hit();
            }
            else if (_canSeeTarget)
            {
                _canSeeTarget = false;
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
        ReduceSliderValue(0.01f);
        flashlight.intensity -= intensityTimeSpeed; 
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 3;
        wallFlashLight.pointLightOuterRadius = 3;
        wallFlashLight.pointLightInnerRadius = 3;
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
    }

    // Set flashlight settings for concentrated light mode
    private void ConcentrateLight()
    {
        ReduceSliderValue(0.1f);
        flashlight.intensity += intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 3;
        wallFlashLight.pointLightOuterRadius = 3;
        wallFlashLight.pointLightInnerRadius = 3;
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

    public void ReduceSliderValue(float _reductionSpeed)
    {
        reductionSpeed = _reductionSpeed;
        currentSliderValue -= reductionSpeed;
        slider.value = currentSliderValue;
        if (currentSliderValue <= 0)
        {
            flashlight.gameObject.SetActive(false);
            wallFlashLight.gameObject.SetActive(false);
            currentSliderValue = 0;
        }
        else
        {
            flashlight.gameObject.SetActive(true);
            wallFlashLight.gameObject.SetActive(true);
        }
            
    }

    // Function to get current energy level
    public float GetEnergy()
    {
        return energy;
    }
}
