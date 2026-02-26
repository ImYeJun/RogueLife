using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : MonoBehaviour
{
    private const string BGM_VOLUME = "BgmVolume";

    [SerializeField] private AudioMixer audioMixer;
    private AudioClip currentBgm;
    private AudioSource audioSource;

    public float Volume { 
        get
        {
            float result;
            audioMixer.GetFloat(BGM_VOLUME, out result);

            return result;
        }
        set
        {
            audioMixer.SetFloat(BGM_VOLUME, value);
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip bgm)
    {
        audioSource.Stop();

        currentBgm = bgm;
        audioSource.clip = currentBgm;

        audioSource.Play();
    }
}