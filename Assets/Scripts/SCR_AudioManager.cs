using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_AudioManager : MonoBehaviour
{
    [Header("--------------------- Audio Sources --------------------- ")]
    [SerializeField] AudioSource[] musicSources;
    [SerializeField] AudioSource[] sfxSources;

    [Header("---------------------- Audio Clip ---------------------- ")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip dash;
    public AudioClip hit;
    public AudioClip pickup;
    public AudioClip build;
    public AudioClip menuAcceptSound;
    
    public static SCR_AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSources[0].clip = background;
        musicSources[0].Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        sfxSources[0].PlayOneShot(clip);
    }
}
