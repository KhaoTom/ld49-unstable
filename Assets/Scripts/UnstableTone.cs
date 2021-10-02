using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableTone : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0.1f, 4f)]
    public float initialPitch = 1;

    [Range(0.1f, 10f)]
    public float driftDuration = 1;
    public float driftScale = 1;
    public AnimationCurve drift;

    [Range(1f, 10f)]
    public float stableDuration = 5;

    private float currentStableDuration = 0;
    private float driftElapsedTime;
    private float stabilityElapsedTime;

    private void Start()
    {
        ResetStability();
    }

    private void Update()
    {
        stabilityElapsedTime += Time.deltaTime;
        if (stabilityElapsedTime > currentStableDuration)
        {
            driftElapsedTime = (driftElapsedTime + Time.deltaTime) % driftDuration;
            audioSource.pitch = initialPitch + drift.Evaluate(driftElapsedTime / driftDuration) * driftScale;
        }
    }

    public void ResetStability()
    {
        audioSource.pitch = initialPitch;
        stabilityElapsedTime = 0;
        driftElapsedTime = 0;
        currentStableDuration = stableDuration + Random.Range(-(stableDuration / 2), stableDuration / 2);
    }
}
