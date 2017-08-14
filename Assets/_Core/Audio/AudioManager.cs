using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float audioVolume;

    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clip;
        audioSource.volume = audioVolume;

        clip = GetComponent<AudioClip>();

        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
