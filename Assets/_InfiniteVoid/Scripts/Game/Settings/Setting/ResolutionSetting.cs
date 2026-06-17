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
        private int _appliedResolutionIndex;
        private FullScreenMode _currentScreenMode;

        private IDisposable _disposable;

        public ResolutionSetting(IReadOnlyList<GameResolution> resolutions, int defaultResolutionIndex, Subject<(FullScreenMode,bool)> screenModeChange)
        {
            _resolutions = resolutions;
            _defaultResolutionIndex = defaultResolutionIndex;
            _currentScreenMode = FullScreenMode.FullScreenWindow;

            _currentResolutionIndex = defaultResolutionIndex;
            _appliedResolutionIndex = defaultResolutionIndex;

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

            Apply();
        }

        public string GetNameOfValue() => _resolutions[_currentResolutionIndex].ToString();
        public object GetValue() => _currentResolutionIndex;

        public bool IsMaxValue() => _currentResolutionIndex >= _resolutions.Count - 1;
        public bool IsMinValue() => _currentResolutionIndex == 0;
        public bool IsCurrentValueApplied() => _currentResolutionIndex == _appliedResolutionIndex;
        public float GetCurrentValueDifference() => (float)_currentResolutionIndex/(float)(_resolutions.Count - 1);

        public object ToNextValue(bool applyChanges = true)
        {
            if (IsMaxValue()) return _currentResolutionIndex;

            _currentResolutionIndex++;

            if (applyChanges) Apply();

            return _currentResolutionIndex;
        }

        public object ToPreviousValue(bool applyChanges = true)
        {
            if (IsMinValue()) return _currentResolutionIndex;

            _currentResolutionIndex--;

            if (applyChanges) Apply();

            return _currentResolutionIndex;
        }

        public void Apply()
        {
            _appliedResolutionIndex = _currentResolutionIndex;

            var resolution = _resolutions[_currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, _currentScreenMode);
        }

        public void ResetToApplied(bool applyChanges = false)
        {
            _currentResolutionIndex = _appliedResolutionIndex;

            if (applyChanges) Apply();
        }

        public void ResetToDefault()
        {
            _currentResolutionIndex = _defaultResolutionIndex;

            Apply();
        }

        private void OnScreenModeChange(FullScreenMode mode, bool isNeedToUpdate)
        {
            _currentScreenMode = mode;

            if (isNeedToUpdate) Apply();
        }
    }
}
