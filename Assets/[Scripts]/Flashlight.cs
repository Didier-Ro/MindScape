using System;
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

    [SerializeField] private float angle;
    private bool _canSeeTarget = false;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float radius;

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
            EnemyLanternCheck();
        }
    }

    private void EnemyLanternCheck()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
        if (rangeCheck.Length == 0) return;
        Transform target = rangeCheck[0].transform;
        Vector2 directionTarget = (target.position - transform.position).normalized;
        if (Vector2.Angle(transform.forward, directionTarget) < angle / 2) //Verify if the plauer is in the designed fov
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position); //Minium distance to see the target
            _canSeeTarget = !Physics2D.Raycast(transform.position, directionTarget, distanceToTarget, obstructionMask);
            if (!_canSeeTarget) return;
            rangeCheck[0].GetComponent<Ikillable>().Hit();
        }
        else if (_canSeeTarget)
        {
            _canSeeTarget = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;

        float halfFOV = angle / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.forward);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.forward);

        Vector3 leftRayDirection = leftRayRotation * transform.right;
        Vector3 rightRayDirection = rightRayRotation * transform.right;

        // Dibujar solo el gizmo del ángulo de visión
        Gizmos.DrawLine( transform.position, transform.position + leftRayDirection * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * radius);
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
        ReduceSliderValue(0.5f);
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
            currentSliderValue = 0;
        }
        else
            flashlight.gameObject.SetActive(true);
    }

    // Function to get current energy level
    public float GetEnergy()
    {
        return energy;
    }
}
