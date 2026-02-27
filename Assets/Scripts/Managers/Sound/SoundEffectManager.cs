using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    private const string SOUND_EFFECT_VOLUME = "SoundEffectVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private int initialPoolCount;
    [SerializeField] private AudioMixerGroup soundEffectAudioMixerGroup;
    [SerializeField, Min(0)] private float pitchShakeRange;

    private List<AudioSource> pool = new List<AudioSource>();

    public float Volume { 
        get
        {
            float result;
            audioMixer.GetFloat(SOUND_EFFECT_VOLUME, out result);

            return result;
        }
        set
        {
            audioMixer.SetFloat(SOUND_EFFECT_VOLUME, value);
        }
    }

    private void Awake() {
        for (int i = 0; i < initialPoolCount; i++)
        {
            CreateAudioMixer();
        }
    }

    public void Play(AudioClip soundEffect)
    {
        var audioSource = GetAvailableAudioSource();

        audioSource.PlayOneShot(soundEffect);
    }
    public void PlayWithRandomPitch(AudioClip souneEffect)
    {
        PlayWithRandomPitch(souneEffect, pitchShakeRange);
    }
    public void PlayWithRandomPitch(AudioClip soundEffect, float pitchShakeRange)
    {
        var audioSource = GetAvailableAudioSource();

        var pitchShakeValue = UnityEngine.Random.Range(-pitchShakeRange, pitchShakeRange);

        var originalPitch = audioSource.pitch;

        var newPitch = originalPitch + pitchShakeValue;
        audioSource.pitch = newPitch;
        audioSource.PlayOneShot(soundEffect);

        audioSource.pitch = originalPitch;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var audioSource in pool)
        {
            if (!audioSource.isPlaying) { return audioSource; }
        }

        return CreateAudioMixer();
    }

    private AudioSource CreateAudioMixer()
    {
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = soundEffectAudioMixerGroup;

        pool.Add(audioSource);

        return audioSource;
    }
}