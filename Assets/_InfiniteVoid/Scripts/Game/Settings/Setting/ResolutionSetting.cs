using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class ResolutionSetting : ISetting, IDisposable
    {
        private readonly IReadOnlyList<GameResolution> _resolutions;
        private readonly int _defaultResolutionIndex;

        private int _currentResolutionIndex;
        private int _savedResolutionIndex;
        private FullScreenMode _currentScreenMode;

        private IDisposable _disposable;

        public ResolutionSetting(IReadOnlyList<GameResolution> resolutions, int defaultResolutionIndex, Subject<(FullScreenMode,bool)> screenModeChange)
        {
            _resolutions = resolutions;
            _defaultResolutionIndex = defaultResolutionIndex;
            _currentScreenMode = FullScreenMode.FullScreenWindow;

            _currentResolutionIndex = defaultResolutionIndex;
            _savedResolutionIndex = defaultResolutionIndex;

            _disposable = screenModeChange.Subscribe(value => OnScreenModeChange(value.Item1, value.Item2));
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void SetValue(int value)
        {
            if (value < 0 || value >= _resolutions.Count) return;

            _currentResolutionIndex = value;
            _savedResolutionIndex = value;

            UpdateScreen();
        }

        public string GetNameOfValue() => _resolutions[_currentResolutionIndex].ToString();
        public object GetValue() => _currentResolutionIndex;

        public bool IsMaxValue() => _currentResolutionIndex >= _resolutions.Count - 1;

        public bool IsMinValue() => _currentResolutionIndex == 0;

        public object ToNextValue()
        {
            if (IsMaxValue()) return _currentResolutionIndex;

            _currentResolutionIndex++;

            UpdateScreen();

            return _currentResolutionIndex;
        }

        public object ToPreviousValue()
        {
            if (IsMinValue()) return _currentResolutionIndex;

            _currentResolutionIndex--;

            UpdateScreen();

            return _currentResolutionIndex;
        }

        public void ResetToDefault()
        {
            _currentResolutionIndex = _defaultResolutionIndex;
            _savedResolutionIndex = _defaultResolutionIndex;

            UpdateScreen();
        }

        public void Save(Action<object> onSaved = null)
        {
            _savedResolutionIndex = _currentResolutionIndex;

            onSaved?.Invoke(_savedResolutionIndex);
        }

        public void ResetToSaved()
        {
            _currentResolutionIndex = _savedResolutionIndex;

            UpdateScreen();
        }

        private void OnScreenModeChange(FullScreenMode mode, bool isNeedToUpdate)
        {
            _currentScreenMode = mode;

            if (isNeedToUpdate) UpdateScreen();
        }

        private void UpdateScreen()
        {
            var resolution = _resolutions[_currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, _currentScreenMode);
        }
    }
}
