using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisableParticle : MonoBehaviour
{
    private ParticleSystem particlesystem;

    private void Awake()
    {
        particlesystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        gameObject.SetActive(false);
    }
}