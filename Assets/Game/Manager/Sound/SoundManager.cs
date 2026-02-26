using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonManager<SoundManager>
{
    private const string MASTER_VOLUME = "MasterVolume";

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private BgmManager bgmManager;
    [SerializeField] private SoundEffectManager soundEffectManager;

    public float MasterVolume { 
        get 
        {
            float result;
            audioMixer.GetFloat(MASTER_VOLUME, out result);

            return result;
        }
        set
        {
            audioMixer.SetFloat(MASTER_VOLUME, NormalizeVolume(value));
        }
    }
    public float BgmVolume { get => bgmManager.Volume; set => bgmManager.Volume = NormalizeVolume(value); }
    public float SoundEffectVolume { get => soundEffectManager.Volume; set => soundEffectManager.Volume = NormalizeVolume(value); }
    private float NormalizeVolume(float value)
    {
        value = Mathf.Clamp01(value);

        return value == 0 ? -80f : Mathf.Log10(value) * 20;
    }

    public void PlayeBgm(AudioClip bgm) { bgmManager.Play(bgm); }
    public void PlayeSoundEffect(AudioClip bgm) { bgmManager.Play(bgm); }
    public void PlaySoundEffectWithRandomPitch(AudioClip soundEffect) { soundEffectManager.PlayWithRandomPitch(soundEffect); }
    public void PlaySoundEffectWithRandomPitch(AudioClip soundEffect, float pitchShakeRange) { soundEffectManager.PlayWithRandomPitch(soundEffect, pitchShakeRange); }
} 