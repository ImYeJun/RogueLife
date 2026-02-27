using UnityEngine;
using UnityEngine.UI;

namespace UI.Global
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider soundEffectVolumeSlider;

        private void OnEnable()
        {
            masterVolumeSlider.value = SoundManager.Instance.MasterVolume;
            bgmVolumeSlider.value = SoundManager.Instance.BgmVolume;
            soundEffectVolumeSlider.value = SoundManager.Instance.SoundEffectVolume;
        }
    }
}