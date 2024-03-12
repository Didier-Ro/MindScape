using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    #region Singleton
    private static Flashlight instance;
    public static Flashlight GetInstance()
    {
        return instance;
    }
    #endregion

    public Slider slider;
    [SerializeField] private Light2D flashlight;
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
    [SerializeField] private float angle;
    private bool canSeeTarget = false;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float radius;
    public float currentSliderValue = 100f;
    private float reductionSpeed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeFlashlight();
        LightSetUp();
    }

    void FixedUpdate()
    {
        HandleInput();
    }

    private void Update()
    {
        if (InputManager.GetInstance().FlashligthInput())
        {
          GameManager.GetInstance().ToggleFlashing();
        }
    }

    // Initialize the flashlight settings
    private void InitializeFlashlight()
    {
        CircleLight();
    }

    // Handle input to toggle flashlight mode
    private void HandleInput()
    {
        if (!GameManager.GetInstance().ReturnFlashing())
        {
            CircleLight();
        }
        else
        {
            ConcentrateLight();
            EnemyLanternCheck();
        }
    }

    // Method to check if an Enemy is within the flashlight's angle
    private void EnemyLanternCheck()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);
        if (rangeCheck.Length == 0) return;
        Transform target = rangeCheck[0].transform;
        Vector2 directionTarget = (target.position - transform.position).normalized;
        if (Vector2.Angle(transform.forward, directionTarget) < angle / 2)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            canSeeTarget = !Physics2D.Raycast(transform.position, directionTarget, distanceToTarget, obstructionMask);
            if (!canSeeTarget) return;
            rangeCheck[0].GetComponent<Ikillable>().Hit();
        }
        else if (canSeeTarget)
        {
            canSeeTarget = false;
        }
    }

    // Set flashlight settings for circle light mode
    private void CircleLight()
    {
        ReduceSliderValue(0.01f);
        flashlight.intensity -= intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 3;
        flashlight.pointLightInnerAngle += lightInnerAngleTimeSpeed;
        flashlight.pointLightOuterAngle += lightOuterAngleTimeSpeed;

        if (flashlight.intensity <= minLightIntensity)
        {
            flashlight.intensity = minLightIntensity;
        }
        if (flashlight.pointLightInnerAngle >= maxPointLightInnerAngle)
        {
            flashlight.pointLightInnerAngle = maxPointLightInnerAngle;
        }
        if (flashlight.pointLightOuterAngle >= maxPointLightOuterAngle)
        {
            flashlight.pointLightOuterAngle = maxPointLightOuterAngle;
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
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * radius);
    }

    // Set flashlight settings for concentrated light mode
    private void ConcentrateLight()
    {
        ReduceSliderValue(0.05f);
        flashlight.intensity += 2 * intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 3;
        flashlight.pointLightInnerAngle -= 2 * lightInnerAngleTimeSpeed;
        flashlight.pointLightOuterAngle -= 2 * lightOuterAngleTimeSpeed;

        if (flashlight.intensity >= maxLightIntensity)
        {
            flashlight.intensity = maxLightIntensity;
        }
        if (flashlight.pointLightInnerAngle <= minPointLightInnerAngle)
        {
            flashlight.pointLightInnerAngle = minPointLightInnerAngle;
        }
        if (flashlight.pointLightOuterAngle <= minPointLightOuterAngle)
        {
            flashlight.pointLightOuterAngle = minPointLightOuterAngle;
        }
    }
    

    // Reduce the slider value based on frames
    public void ReduceSliderValue(float _reductionSpeed)
    {
        reductionSpeed = _reductionSpeed;
        currentSliderValue -= reductionSpeed;
        slider.value = currentSliderValue;
        if(slider.value <= 0)
            flashlight.gameObject.SetActive(false);
        else 
            flashlight.gameObject.SetActive(true);
    }

    // Set up initial flashlight settings
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
}
