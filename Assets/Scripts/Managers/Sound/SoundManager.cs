using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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

            return NormalizeDecibelToVolume(result);
        }
        set
        {
            audioMixer.SetFloat(MASTER_VOLUME, NormalizeVolumeToDecibel(value));
        }
    }
    public float BgmVolume { get => NormalizeDecibelToVolume(bgmManager.Volume); set => bgmManager.Volume = NormalizeVolumeToDecibel(value); }
    public float SoundEffectVolume { get => NormalizeDecibelToVolume(soundEffectManager.Volume); set => soundEffectManager.Volume = NormalizeVolumeToDecibel(value); }
    private float NormalizeDecibelToVolume(float value)
    {
        return Mathf.Pow(10f, value/20f);
    }
    private float NormalizeVolumeToDecibel(float value)
    {
        value = Mathf.Clamp01(value);

        return value == 0 ? -80f : Mathf.Log10(value) * 20;
    }

    public void PlayeBgm(AudioData bgm) { bgmManager.Play(bgm.Clip); }
    public void PlayeSoundEffect(AudioData bgm) { bgmManager.Play(bgm.Clip); }
    public void PlaySoundEffectWithRandomPitch(AudioData soundEffect) { soundEffectManager.PlayWithRandomPitch(soundEffect.Clip); }
    public void PlaySoundEffectWithRandomPitch(AudioData soundEffect, float pitchShakeRange) { soundEffectManager.PlayWithRandomPitch(soundEffect.Clip, pitchShakeRange); }
} 