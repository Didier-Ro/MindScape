using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowEffect : MonoBehaviour, Ikillable
{
    private Vector3 offset = new Vector3(-0.1f, -0.1f);
    [SerializeField] private Material material;
    [SerializeField] private Light2D light2D;

    GameObject shadow;

    Vector3 lightDirection;
    Vector3 shadowPosition;

    float distanceToLight;

    float minDistanceToShowShadow = 0.5f; // Distancia m�nima para que la sombra sea visible
    float maxDistanceToShowShadow = 2.0f; // Distancia m�xima a la que la sombra estar� completamente visible
    float maxShadowScale = 1f; // Escala m�xima de la sombra
    float shadowScale;
    float startFadingDistance = 5.0f;
    float maxDistanceToHideShadow = 10.0f;
    float shadowOpacity = 1.0f;

    public Light_State lightState = Light_State.DEPLOY;

    public bool isIlluminated = false;

    void Start()
    {
        light2D = GameManager.GetInstance().GetLightReference();
        material = GameManager.GetInstance().GetShadowMaterial();

        shadow = new GameObject("Shadow");
        shadow.transform.parent = transform;

        shadow.transform.localPosition = offset;
        shadow.transform.localRotation = Quaternion.identity;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = shadow.AddComponent<SpriteRenderer>();
        sr.sprite = renderer.sprite;
        sr.material = material;

        sr.sortingLayerName = renderer.sortingLayerName;
        sr.sortingOrder = renderer.sortingOrder - 1;

    }
    private void Update()
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

    private void LateUpdate()
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

    public void DrawShadow()
    {
        lightDirection = light2D.transform.position - transform.position;
        distanceToLight = lightDirection.magnitude;

        if (distanceToLight > minDistanceToShowShadow && distanceToLight < maxDistanceToHideShadow)
        {
            shadow.SetActive(true);

            // Calculamos la escala de la sombra basada en la distancia a la luz
            shadowScale = Mathf.Clamp01((distanceToLight - minDistanceToShowShadow) / (maxDistanceToShowShadow - minDistanceToShowShadow)) * maxShadowScale;

            // Calculamos la opacidad de la sombra en funci�n de la distancia

            if (distanceToLight > startFadingDistance)
            {
                shadowOpacity = 1 - ((distanceToLight - startFadingDistance) / (maxDistanceToHideShadow - startFadingDistance));
            }

            // Aplicamos la escala y la opacidad a la sombra
            shadow.transform.localScale = new Vector3(shadowScale, shadowScale, 1f);
            Color shadowColor = material.color;
            shadowColor.a = shadowOpacity;
            material.color = shadowColor;

            // Calculamos la posici�n de la sombra detr�s del objeto
            shadowPosition = -lightDirection.normalized * shadowScale;
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

    public void Hit(Transform player)
    {
        isIlluminated = true;
        Debug.Log("Iluminado");
    }

    public void UnHit(Transform player)
    {
        isIlluminated = false;
    }
}
public enum Light_State
{
    DEPLOY,
    CONCETRATE
}