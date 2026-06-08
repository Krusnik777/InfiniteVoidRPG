using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class GraphicsController
    {
        private IGraphicsSettingsConfig _defaultConfig;

        private int _currentResolutionIndex;
        private FullScreenMode _currentMode = FullScreenMode.FullScreenWindow;

        public GraphicsController(IGraphicsSettingsConfig config)
        {
            _defaultConfig = config;
            _currentResolutionIndex = config.DefaultResolutionIndex;
        }

        public void SetVSync(bool state)
        {
            QualitySettings.vSyncCount = state ? 1 : 0;
        }

        public void SetScreenMode(ApplicationScreenMode screenMode, bool updateScreenImmediately = true)
        {
            _currentMode = screenMode switch
            {
                ApplicationScreenMode.ExclusiveFullScreen => FullScreenMode.ExclusiveFullScreen,
                ApplicationScreenMode.FullScreenWindow => FullScreenMode.FullScreenWindow,
                ApplicationScreenMode.Windowed => FullScreenMode.Windowed,
                _ => FullScreenMode.FullScreenWindow,
            };

            if (updateScreenImmediately) UpdateScreenParameters();
        }

        public void SetResolution(int index)
        {
            _currentResolutionIndex = index;

            UpdateScreenParameters();
        }

        public void ResetToDefaults()
        {
            SetVSync(_defaultConfig.VSyncEnabled);
            SetScreenMode(_defaultConfig.ScreenMode, false);
            SetResolution(_defaultConfig.DefaultResolutionIndex);
        }

        private void UpdateScreenParameters()
        {
            var resolution = _defaultConfig.Resolutions[_currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, _currentMode);
        }
    }
}
