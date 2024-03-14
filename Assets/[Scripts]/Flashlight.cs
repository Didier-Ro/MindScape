using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
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
    

    [SerializeField] private float angleRange;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float radius;
    private bool _canSeeTarget = false;
    public float angleDegrees;
    public float lightAngle;
   
    
    [Header("RotateLight")]
    private Vector2 lastMousePosition = Vector2.zero;
    [SerializeField] private Transform flashLightTransform;
    [SerializeField] private float rotateSpeed = default;
    [SerializeField] private Transform wallFlashLightTransform;
    [SerializeField] private Camera camera = default;

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

    private void Update()
    {
        UpdateEnergyUI();
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
        // Reduce energy based on flashlight mode
        ReduceEnergy();
    }

    // Update the energy UI slider
    private void UpdateEnergyUI()
    {
        slider.value = energy;
    }

    private void RotateLight()
    {
        Vector2 inputLight = camera.ScreenToWorldPoint(InputManager.GetInstance().MoveLightInput());
        Vector2 direction = (inputLight - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flashLightTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        wallFlashLightTransform.rotation  = Quaternion.AngleAxis(angle, Vector3.forward);
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
        if (rangeCheck.Length == 0) return;
        foreach (Collider2D col in rangeCheck)
        {
            Vector2 direction = col.transform.position - transform.position.normalized;
            direction.x = ReduceErrorZero(direction.x);
            direction.y = ReduceErrorZero(direction.y);
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            angleDegrees = Mathf.Repeat(angleRadians * Mathf.Rad2Deg, 360);
            lightAngle = Mathf.Repeat(flashLightTransform.rotation.eulerAngles.z, 360); 
            Debug.Log( "rango mayor " + Mathf.Repeat(lightAngle + angleRange, 360) + " angulo "  + angleDegrees +  " rango menor " + Mathf.Repeat(lightAngle - angleRange, 360));
            if (Mathf.Repeat(lightAngle + angleRange, 360)< angleDegrees && angleDegrees > Mathf.Repeat(lightAngle - angleRange, 360))
            {
                Debug.Log("le hizo daño");
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

    // Reduce energy based on flashlight mode
    private void ReduceEnergy()
    {
        if (!flashing)
        {
            energy += Time.deltaTime / 2; // Reduce energy slowly
        }
        else
        {
            energy -= Time.deltaTime / 3; // Reduce energy faster
        }

        // Clamp energy to ensure it stays within valid range
        energy = Mathf.Clamp(energy, 0f, slider.maxValue);
    }

    // Function to get current energy level
    public float GetEnergy()
    {
        return energy;
    }
}
