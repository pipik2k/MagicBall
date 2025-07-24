using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallSound : MonoBehaviour
{
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private AudioSource audioSource;

    private float lastPlayTime = -1f;
    private const float minInterval = 0.1f;

    public void PlaySoundEffect()
    {
        if (audioSource != null && soundEffect != null)
        {
            float currentTime = Time.time;
            if (currentTime - lastPlayTime >= minInterval)
            {
                audioSource.PlayOneShot(soundEffect);
                lastPlayTime = currentTime;
            }
        }
    }
}
