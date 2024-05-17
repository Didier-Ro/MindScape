using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowEffect : MonoBehaviour, Ikillable
{
    private Vector3 offset = new Vector3(-0.1f, -0.1f);
    [SerializeField] private Material material;
    [SerializeField] private Light2D light2D;

    public GameObject shadow;

    public Vector3 lightDirection;
    Vector3 shadowPosition;

    public float distanceToLight;

    float minDistanceToShowShadow = 0.5f; // Distancia m�nima para que la sombra sea visible
    float maxDistanceToShowShadow = 2.0f; // Distancia m�xima a la que la sombra estar� completamente visible
    float maxShadowScale = 1f; // Escala m�xima de la sombra
    float shadowScale;
    float startFadingDistance = 10.0f;
    float maxDistanceToHideShadow = 10.0f;
    float shadowOpacity = 1.0f;

    public Light_State lightState = Light_State.DEPLOY;
    private LIGHT_ENERGY_STATE lighEnergy;
    public bool isIlluminated = false;

    [SerializeField] private BoxFalling box;

    private bool canCreateOtherShadow = false;
    Color shadowColor;

    public float dis1;
    public float dis2;


    void Start()
    {
        light2D = GameManager.GetInstance().GetLightReference();
        material = GameManager.GetInstance().GetShadowMaterial();
        shadowColor = material.color;
        CreateShadow();
        box.OnBoxStateChange += OnBoxStateChanged;
        SubscribeToFlashlight();
    }

    private void Update()
    {
        if (lighEnergy == LIGHT_ENERGY_STATE.ON)
        {          
            if (!GameManager.GetInstance().GetFlashing())
            {
                lightState = Light_State.DEPLOY;
            }
            else
            {
                lightState = Light_State.CONCETRATE;
            }
        }
    }

    private void FixedUpdate()
    {
        if(lighEnergy == LIGHT_ENERGY_STATE.ON)
        {
            if(shadow != null)
            {
                if (lightState == Light_State.DEPLOY)
                {
                    isIlluminated = false;
                    DrawShadow();
                }

                if (lightState == Light_State.CONCETRATE)
                {
                    if (isIlluminated)
                        DrawShadow();
                    else
                    {
                        shadow.SetActive(false);
                    }
                }
            }
        }
    }


    private void OnDisable()
    {
        box.OnBoxStateChange -= OnBoxStateChanged;
        DesubscribeToFlashLight();
    }

    public void DrawShadow()
    {
        lightDirection = light2D.transform.position - transform.position;
        distanceToLight = lightDirection.magnitude;
        
        if (distanceToLight > minDistanceToShowShadow && distanceToLight < maxDistanceToHideShadow)
        {
            shadow.SetActive(true);

            // Calculamos la escala de la sombra basada en la distancia a la luz
            dis1 = distanceToLight - minDistanceToShowShadow;
            dis2 = maxDistanceToShowShadow - minDistanceToShowShadow;
            shadowScale = Mathf.Clamp01(dis1 / dis2) * maxShadowScale;

            // Calculamos la opacidad de la sombra en funci�n de la distancia

            if (distanceToLight > startFadingDistance)
            {
                shadowOpacity = - ((distanceToLight - startFadingDistance) / (maxDistanceToHideShadow - startFadingDistance));
            }

            // Aplicamos la escala y la opacidad a la sombra
           // shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 1f);
            
            shadowColor.a = shadowOpacity;
            material.color = shadowColor;

            // Calculamos la posici�n de la sombra detr�s del objeto
            shadowPosition = -lightDirection.normalized * shadowScale * 0.5f;
            shadow.transform.localPosition = shadowPosition;
            // Ajustamos la rotaci�n de la sombra seg�n la rotaci�n del objeto
            shadow.transform.eulerAngles = transform.eulerAngles;
        }
        else
        {
            // Si la luz est� muy cerca o muy lejos, ocultamos la sombra
            shadow.SetActive(false);
        }
    }

    private void DeleteShadow()
    {
        shadow.SetActive(false);
    }

    private void ShowShadow()
    {
        shadow.SetActive(true);
    }

    private void CreateShadow()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = shadow.AddComponent<SpriteRenderer>();
        sr.sprite = renderer.sprite;
        sr.material = material;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;
    }

    public void OnBoxStateChanged(BOX_STATE _newwBoxState)
    {
        switch (_newwBoxState)
        {
            case BOX_STATE.FALLING:
                DeleteShadow();
                break;
            case BOX_STATE.SPAWNING:
                ShowShadow(); 
                break;
        }
    }
    public void Hit(Transform player)
    {
        isIlluminated = true;
        
    }

    public void UnHit(Transform player)
    {
        isIlluminated = false;
    }

    private void SubscribeToFlashlight()
    {
        Flashlight.GetInstance().OnLightEnergyChange += OnLightEnergyChanged;
        OnLightEnergyChanged(Flashlight.GetInstance().GetLightEnergyState());
    }

    private void DesubscribeToFlashLight()
    {
        Flashlight.GetInstance().OnLightEnergyChange -= OnLightEnergyChanged;
    }

    private void OnLightEnergyChanged(LIGHT_ENERGY_STATE _energyState)
    {
        lighEnergy = _energyState;

        switch (lighEnergy)
        {
            case LIGHT_ENERGY_STATE.ON:
                ShowShadow();
                break;
            case LIGHT_ENERGY_STATE.OFF:
                DeleteShadow();
                break;
        }
    }
}

public enum Light_State
{
    DEPLOY,
    CONCETRATE
}