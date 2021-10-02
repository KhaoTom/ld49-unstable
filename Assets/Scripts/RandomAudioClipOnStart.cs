using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClipOnStart : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Start()
    {
        PlayRandomClip();
    }

    private void PlayRandomClip()
    {
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        if (clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

}
