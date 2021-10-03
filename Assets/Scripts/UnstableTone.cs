using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnstableTone : MonoBehaviour
{
    public AudioSource audioSource;
    public ParticleSystem mainParticle;
    public ParticleSystemRenderer mainParticleRenderer;
    public Gradient particleColorDrift;

    [Range(0.1f, 4f)]
    public float initialPitch = 1;

    [Range(0.1f, 10f)]
    public float driftDuration = 1;
    public float driftScale = 1;
    public AnimationCurve drift;

    [Range(1f, 10f)]
    public float stableDuration = 5;

    public UnityEvent onHit;

    private float currentStableDuration = 0;
    private float driftElapsedTime;
    private float stabilityElapsedTime;

    public float Instability { get => Mathf.Abs(initialPitch - audioSource.pitch); }

    public bool hold = true;

    private void Start()
    {
        ResetStability();
    }

    private void Update()
    {
        if (!hold)
        {
            stabilityElapsedTime += Time.deltaTime;
            if (stabilityElapsedTime > currentStableDuration)
            {
                driftElapsedTime = driftElapsedTime + Time.deltaTime;
            }

            audioSource.pitch = initialPitch + drift.Evaluate(driftElapsedTime % driftDuration / driftDuration) * driftScale;
            mainParticleRenderer.material.color = particleColorDrift.Evaluate(Mathf.Clamp01(driftElapsedTime));
        }
    }

    public void ResetStability()
    {
        audioSource.pitch = initialPitch;
        mainParticleRenderer.material.color = particleColorDrift.Evaluate(0);
        stabilityElapsedTime = 0;
        driftElapsedTime = 0;
        currentStableDuration = stableDuration + Random.Range(-(stableDuration / 2), stableDuration / 2);
    }

    public void OnCollisionEnter(Collision collision)
    {
        ResetStability();
        onHit.Invoke();
    }
}
