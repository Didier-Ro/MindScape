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
    [SerializeField] private float energy = 100f; // Initial energy value
    private bool flashing = false;

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

        if (!flashing)
        {
            CircleLight();
        }
        else
        {
            ConcentrateLight();
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

    // Set flashlight settings for concentrated light mode
    private void ConcentrateLight()
    {
        flashlight.intensity += intensityTimeSpeed;
        flashlight.pointLightOuterRadius = 3;
        flashlight.pointLightInnerRadius = 3;
        flashlight.pointLightInnerAngle -= lightInnerAngleTimeSpeed;
        flashlight.pointLightOuterAngle -= lightOuterAngleTimeSpeed;

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

    // Toggle between flashlight modes
    public void ToggleFlashing()
    {
        flashing = !flashing;
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
