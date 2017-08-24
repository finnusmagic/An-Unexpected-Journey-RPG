﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Space(10)]
    public bool loop = false;
    public bool deactivate = false;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        if (!deactivate)
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();

            if (deactivate)
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        source.Stop();
    }
}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip clip;
    [Space(10)]
    public bool loop = false;
    public bool deactivate = false;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        if (!deactivate)
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();

            if (deactivate)
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Sound[] sounds;
    [SerializeField] Music[] musicTracks;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource (_go.AddComponent<AudioSource>());
        }
        for (int i = 0; i < musicTracks.Length; i++)
        {
            GameObject _go = new GameObject("Music:" + i + "_" + musicTracks[i].name);
            _go.transform.SetParent(this.transform);
            musicTracks[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No Sound found.");
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No Sound found.");
    }

    public void PlayMusic(string _name)
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            if (musicTracks[i].name == _name)
            {
                musicTracks[i].Play();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No Music found.");
    }

    public void StopMusic()
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            foreach (Music track in musicTracks)
            {
                musicTracks[i].Stop();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No Music found.");
    }
}
