using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowEffect : MonoBehaviour
{
    public Vector3 offset = new Vector3(-0.1f, -0.1f);
    public Material material;

    GameObject shadow;

    public Light2D light2D;


    void Start()
    {
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

    private void LateUpdate()
    {
        shadow.transform.localPosition = -light2D.transform.position.normalized /*+ transform.position.normalized*/;
        shadow.transform.eulerAngles = transform.eulerAngles;

    }
}
