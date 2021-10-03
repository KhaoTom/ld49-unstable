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
    public UnityEvent onStart;
    public UnityEvent<Score> onCompleteEvent;

    private float timeRemaining;
    UnstableTone[] tones;
    float instability;
    int hits;
    bool started = false;

    private void Start()
    {
        timeRemaining = duration;
        foreach (var tone in tones)
        {
            tone.onHit.AddListener(() => hits++);
        }

        instability = 0;
        hits = 0;
        timerText.text = timeRemaining.ToString();
    }

    private void Update()
    {
        if (!started)
        {
            started = true;
            onStart.Invoke();
        }

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

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            timeRemaining = 0f;
        }
#endif
    }

    private void OnEnable()
    {
        tones = FindObjectsOfType<UnstableTone>();
        foreach (var tone in tones)
        {
            tone.hold = false;
        }
    }

    private void OnDisable()
    {
        foreach (var tone in tones)
        {
            tone.hold = true;
        }
    }
}

[System.Serializable]
public struct Score
{
    public float instability;
    public int hits;
}
