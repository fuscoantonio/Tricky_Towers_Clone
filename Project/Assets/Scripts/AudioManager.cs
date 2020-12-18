using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _timeOfLastAudio = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float pitch)
    {
        float timeDifference = Time.time - _timeOfLastAudio;
        if (timeDifference > 0.2)
        {
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(clip);
            _timeOfLastAudio = Time.time;
        }
    }
}
