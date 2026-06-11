using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class ScreenModeSetting : ISetting
    {
        private readonly int _defaultModeIndex;

        private int _currentModeIndex;
        private int _appliedModeIndex;

        private Subject<(FullScreenMode,bool)> _screenModeChange;

        public ScreenModeSetting(int defaultModeIndex, Subject<(FullScreenMode,bool)> screenModeChange)
        {
            _defaultModeIndex = defaultModeIndex;
            _screenModeChange = screenModeChange;

            _currentModeIndex = _defaultModeIndex;
            _appliedModeIndex = _defaultModeIndex;
        }

        public void SetValue(int index, bool needToUpdate)
        {
            _currentModeIndex = index;
            _appliedModeIndex = _currentModeIndex;

            UpdateScreenMode(needToUpdate);
        }

        public string GetNameOfValue()
        {
            ApplicationScreenMode mode = (ApplicationScreenMode) _currentModeIndex;

            return mode switch
            {
                ApplicationScreenMode.ExclusiveFullScreen => "FullScreen",
                ApplicationScreenMode.FullScreenWindow => "Borderless",
                ApplicationScreenMode.Windowed => "Windowed",
                _ => ""
            };
        }

        public object GetValue() => _currentModeIndex;
        public bool IsMaxValue() => _currentModeIndex >= System.Enum.GetNames(typeof(ApplicationScreenMode)).Length - 1;
        public bool IsMinValue() => _currentModeIndex == 0;
        public bool IsCurrentValueApplied() => _appliedModeIndex == _currentModeIndex;
        public float GetCurrentValueDifference() => (float)_currentModeIndex/(float)(System.Enum.GetNames(typeof(ApplicationScreenMode)).Length - 1);

        public object ToNextValue(bool applyChanges = true)
        {
            if (IsMaxValue()) return _currentModeIndex;

            _currentModeIndex++;

            if (applyChanges) Apply();

            return _currentModeIndex;
        }

        public object ToPreviousValue(bool applyChanges = true)
        {
            if (IsMinValue()) return _currentModeIndex;

            _currentModeIndex--;

            if (applyChanges) Apply();

            return _currentModeIndex;
        }

        public void Apply()
        {
            _appliedModeIndex = _currentModeIndex;

            UpdateScreenMode();
        }

        public void ResetToDefault()
        {
            _currentModeIndex = _defaultModeIndex;

            Apply();
        }

        private void UpdateScreenMode(bool needToUpdate = true)
        {
            var currentScreenMode = (ApplicationScreenMode) _currentModeIndex;

            var engineScreenMode = currentScreenMode switch
            {
                ApplicationScreenMode.ExclusiveFullScreen => FullScreenMode.ExclusiveFullScreen,
                ApplicationScreenMode.FullScreenWindow => FullScreenMode.FullScreenWindow,
                ApplicationScreenMode.Windowed => FullScreenMode.Windowed,
                _ => FullScreenMode.FullScreenWindow,
            };

            _screenModeChange.OnNext((engineScreenMode, needToUpdate));
        }
    }
}
