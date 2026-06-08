using InfiniteVoidRPG.Game.Data;
using InfiniteVoidRPG.Game.Settings;
using Localization;

namespace InfiniteVoidRPG.Game.Services
{
    public class ApplicationControlService
    {
        private IApplicationSettingsDataHandler _applicationSettingsDataHandler;
        private GraphicsController _graphicsController;
        private AudioMixerController _audioMixerController;

        public ApplicationControlService(IApplicationSettingsDataHandler applicationSettingsDataHandler, GraphicsController graphicsController, AudioMixerController audioMixerController)
        {
            _applicationSettingsDataHandler = applicationSettingsDataHandler;
            _graphicsController = graphicsController;
            _audioMixerController = audioMixerController;
        }

        public void Initialize()
        {
            LocalizationSystem.ChangeLanguage((LocalizationLanguage)_applicationSettingsDataHandler.Data.CurrentLanguageIndex);

            _graphicsController.SetVSync(_applicationSettingsDataHandler.Data.Graphics.VSyncState);
            _graphicsController.SetScreenMode((ApplicationScreenMode)_applicationSettingsDataHandler.Data.Graphics.ScreenModeIndex, false);
            _graphicsController.SetResolution(_applicationSettingsDataHandler.Data.Graphics.ResolutionIndex);

            _audioMixerController.SetSFXValue(_applicationSettingsDataHandler.Data.Audio.SFXVolume);
            _audioMixerController.SetBGMValue(_applicationSettingsDataHandler.Data.Audio.BGMVolume);
        }

        public void SaveSettings()
        {
            _applicationSettingsDataHandler.SaveData();
        }

        public void ResetToDefaults()
        {
            LocalizationSystem.SetLanguageBySystem();

            _graphicsController.ResetToDefaults();
            _audioMixerController.ResetToDefaults();

            _applicationSettingsDataHandler.ResetData();
            _applicationSettingsDataHandler.SaveData();
        }

        public void ApplySFXValueChange(int value)
        {
            _audioMixerController.SetSFXValue(value);
            _applicationSettingsDataHandler.Data.Audio.SFXVolume = value;
        }

        public void ApplyBGMValueChange(int value)
        {
            _audioMixerController.SetBGMValue(value);
            _applicationSettingsDataHandler.Data.Audio.BGMVolume = value;
        }

        public void ApplyVSyncChange(bool state)
        {
            _graphicsController.SetVSync(state);
            _applicationSettingsDataHandler.Data.Graphics.VSyncState = state;
        }

        public void ApplyScreenModeChange(ApplicationScreenMode mode)
        {
            _graphicsController.SetScreenMode(mode);
            _applicationSettingsDataHandler.Data.Graphics.ScreenModeIndex = (int)mode;
        }

        public void ApplyResolutionChange(int resolutionIndex)
        {
            _graphicsController.SetResolution(resolutionIndex);
            _applicationSettingsDataHandler.Data.Graphics.ResolutionIndex = resolutionIndex;
        }
    }
}
