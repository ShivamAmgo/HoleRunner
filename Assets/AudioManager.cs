using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] AudioSources;
    
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += FinishLineCross;
        HoleManager.OnROundStart += OnRoundStarted;
    }

    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= FinishLineCross;
        HoleManager.OnROundStart -= OnRoundStarted;
    }

    private void OnRoundStarted()
    {
        AudioSources[AudioSources.Length-1].Play();
    }

    private void FinishLineCross()
    {
        AudioSources[AudioSources.Length-1].Stop();
    }

    public void PlaySound(string SoundType, AudioClip audioClip)
    {
        if (audioClip==null)
        {
            return;
        }
        
        if (SoundType=="Player")
        {
            AudioSources[0].clip = audioClip;
            AudioSources[0].Play();
        }
        else if (SoundType=="Prop")
        {
            AudioSources[1].clip = audioClip;
            AudioSources[1].Play();
        }
        else if (SoundType=="Enemy")
        {
            AudioSources[2].clip = audioClip;
            AudioSources[2].Play();
        }
        
    }
}
