using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Scoring : MonoBehaviour
{
    public Text scoreText;
    public Text timerText;
    public float duration;
    public UnityEvent<Score> onCompleteEvent;

    private float timeRemaining;
    UnstableTone[] tones;
    float instability;
    int hits;

    void Start()
    {
        timeRemaining = duration;
        tones = FindObjectsOfType<UnstableTone>();
        foreach (var tone in tones)
        {
            tone.onHit.AddListener(() => hits++);
        }

        instability = 0;
        hits = 0;
        timerText.text = timeRemaining.ToString();
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        foreach (var tone in tones)
        {
            instability += tone.Instability * Time.deltaTime;
        }

        scoreText.text = instability.ToString("0.000") + " MILLION";
        timerText.text = timeRemaining.ToString("0.00");

        if (timeRemaining <= 0)
        {
            onCompleteEvent.Invoke(new Score() { instability = instability, hits = hits });
            this.enabled = false;
        }
    }
}

[System.Serializable]
public struct Score
{
    public float instability;
    public int hits;
}
