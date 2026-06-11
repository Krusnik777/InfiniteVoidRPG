using InfiniteVoidRPG.Game.Data;
using InfiniteVoidRPG.Game.Settings;
using R3;
using UnityEngine;
using UnityEngine.Audio;

namespace InfiniteVoidRPG.Game.Services
{
    public class ApplicationControlService : System.IDisposable
    {
        private const string _sfxVolumeName = "SFXVolume";
        private const string _bgmVolumeName = "BGMVolume";

        public LanguageSetting LanguageSetting { get; private set; }

        public SoundSetting SFXVolumeSetting { get; private set; }
        public SoundSetting BGMVolumeSetting { get; private set; }

        public VSyncSetting VSyncSetting { get; private set; }
        public ResolutionSetting ResolutionSetting { get; private set; }
        public ScreenModeSetting ScreenModeSetting { get; private set; }

        private IApplicationSettingsDataHandler _applicationSettingsDataHandler;

        private Subject<(FullScreenMode, bool)> _onScreenModeChange;
        
        public ApplicationControlService(IApplicationSettingsDataHandler applicationSettingsDataHandler, 
                                         AudioMixer audioMixer, IAudioSettingsConfig _defaultSoundConfig, 
                                         IGraphicsSettingsConfig _defaultGraphicsConfig)
        {
            _applicationSettingsDataHandler = applicationSettingsDataHandler;

            LanguageSetting = new((int)Localization.LocalizationSystem.CurrentLanguage);
            
            SFXVolumeSetting = new(_sfxVolumeName, audioMixer, _defaultSoundConfig.SFXVolume);
            BGMVolumeSetting = new(_bgmVolumeName, audioMixer, _defaultSoundConfig.BGMVolume);

            _onScreenModeChange = new();

            VSyncSetting = new(_defaultGraphicsConfig.VSyncEnabled);
            ResolutionSetting = new(_defaultGraphicsConfig.Resolutions, _defaultGraphicsConfig.DefaultResolutionIndex, _onScreenModeChange);
            ScreenModeSetting = new((int)_defaultGraphicsConfig.ScreenMode, _onScreenModeChange);
        }

        public void Dispose()
        {
            ResolutionSetting?.Dispose();
        }

        public void Initialize()
        {
            LanguageSetting.SetValue(_applicationSettingsDataHandler.Data.CurrentLanguageIndex);

            SFXVolumeSetting.SetValue(_applicationSettingsDataHandler.Data.Audio.SFXVolume);
            BGMVolumeSetting.SetValue(_applicationSettingsDataHandler.Data.Audio.BGMVolume);

            VSyncSetting.SetValue(_applicationSettingsDataHandler.Data.Graphics.VSyncState);
            ScreenModeSetting.SetValue(_applicationSettingsDataHandler.Data.Graphics.ScreenModeIndex, false);
            ResolutionSetting.SetValue(_applicationSettingsDataHandler.Data.Graphics.ResolutionIndex);
        }

        public void SaveSettings()
        {
            _applicationSettingsDataHandler.Data.CurrentLanguageIndex = (int)LanguageSetting.GetValue();
            
            _applicationSettingsDataHandler.Data.Audio.SFXVolume = (int)SFXVolumeSetting.GetValue();
            _applicationSettingsDataHandler.Data.Audio.BGMVolume = (int)BGMVolumeSetting.GetValue();

            _applicationSettingsDataHandler.Data.Graphics.VSyncState = (bool)VSyncSetting.GetValue();
            _applicationSettingsDataHandler.Data.Graphics.ScreenModeIndex = (int)ScreenModeSetting.GetValue();
            _applicationSettingsDataHandler.Data.Graphics.ResolutionIndex = (int)ResolutionSetting.GetValue();

            _applicationSettingsDataHandler.SaveData();
        }

        public void ResetToDefaults()
        {
            LanguageSetting.ResetToDefault();
            
            SFXVolumeSetting.ResetToDefault();
            BGMVolumeSetting.ResetToDefault();

            VSyncSetting.ResetToDefault();
            ScreenModeSetting.ResetToDefault();
            ResolutionSetting.ResetToDefault();

            _applicationSettingsDataHandler.ResetData();
            _applicationSettingsDataHandler.SaveData();
        }
    }
}
