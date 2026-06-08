using UnityEngine.Audio;

namespace InfiniteVoidRPG.Game.Settings
{
    public class AudioMixerController
    {
        private const string _sfxVolumeName = "SFXVolume";
        private const string _bgmVolumeName = "BGMVolume";

        private AudioMixer _audioMixer;
        private IAudioSettingsConfig _defaultConfig;

        public AudioMixerController(AudioMixer audioMixer, IAudioSettingsConfig config)
        {
            _audioMixer = audioMixer;
            _defaultConfig = config;
        }

        public void SetSFXValue(int value)
        {
            if (value < -80) value = -80;
            if (value > 20) value = 20;

            _audioMixer.SetFloat(_sfxVolumeName, value);
        }

        public void SetBGMValue(int value)
        {
            if (value < -80) value = -80;
            if (value > 20) value = 20;

            _audioMixer.SetFloat(_bgmVolumeName, value);
        }

        public void ResetToDefaults()
        {
            SetSFXValue(_defaultConfig.SFXVolume);
            SetBGMValue(_defaultConfig.BGMVolume);
        }
    }
}
