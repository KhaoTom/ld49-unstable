using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    public Image speakerImage;
    public Text segmentText;


    [System.Serializable]
    public class Segment
    {
        public string speaker;
        public string expression;
        [TextArea]
        public string text;
    }

    public Segment[] segments;

    [System.Serializable]
    public class SpeakerConfig
    {
        public string name;
        public string expression;
        public Sprite sprite;
    }

    public SpeakerConfig[] speakerConfigs;

    public UnityEvent onCompleted;

    public Dictionary<string, Sprite> speakers = new Dictionary<string, Sprite>();

    public bool waitingForInput = false;
    public bool completed = false;
    public int currentSegmentIndex = 0;

    private void Start()
    {
        foreach (var config in speakerConfigs)
        {
            speakers.Add(MakeSpeakerSpriteId(config.name, config.expression), config.sprite);
        }

        if (segments.Length > 0)
        {
            ShowSegment(currentSegmentIndex);
        }
        else
        {
            completed = true;
        }
    }

    private void Update()
    {
        if (completed)
        {
            onCompleted.Invoke();
            this.enabled = false;
        }

        if (waitingForInput && !completed)
        {
            if (Input.anyKeyDown)
            {
                waitingForInput = false;
                ShowNextSegment();
            }
        }
    }

    public string MakeSpeakerSpriteId(string name, string expression)
    {
        return $"{name}.{expression}";
    }

    public void ShowNextSegment()
    {
        currentSegmentIndex++;
        if (currentSegmentIndex >= segments.Length)
        {
            completed = true;
        }
        else
        {
            ShowSegment(currentSegmentIndex);
        }
    }

    public void ShowSegment(int segmentIndex)
    {
        if (segments.Length > segmentIndex)
        {
            var segment = segments[segmentIndex];
            SetSpeaker(segment.speaker, segment.expression);
            SetText(segment.text);
            Invoke("SetWaitForInput", 0.2f);
        }
    }

    public void SetWaitForInput()
    {
        waitingForInput = true;
    }

    public void SetSpeaker(string name, string expression)
    {
        if (speakers.TryGetValue(MakeSpeakerSpriteId(name, expression), out Sprite speakerSprite))
        {
            speakerImage.sprite = speakerSprite;
            speakerImage.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            speakerImage.enabled = false;
            speakerImage.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetText(string text)
    {
        segmentText.text = text;
    }
}
