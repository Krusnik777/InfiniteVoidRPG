using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class ScreenModeSetting : ISetting
    {
        private readonly int _defaultModeIndex;

        private int _currentModeIndex;
        private int _savedModeIndex;

        private Subject<(FullScreenMode,bool)> _screenModeChange;

        public ScreenModeSetting(int defaultModeIndex, Subject<(FullScreenMode,bool)> screenModeChange)
        {
            _defaultModeIndex = defaultModeIndex;
            _screenModeChange = screenModeChange;

            _currentModeIndex = _defaultModeIndex;
            _savedModeIndex = _defaultModeIndex;
        }

        public void SetValue(int index, bool needToUpdate)
        {
            _currentModeIndex = index;
            _savedModeIndex = index;

            UpdateScreenMode(needToUpdate);
        }

        public string GetNameOfValue()
        {
            ApplicationScreenMode mode = (ApplicationScreenMode) _currentModeIndex;

            return mode switch
            {
                ApplicationScreenMode.ExclusiveFullScreen => "Full Screen",
                ApplicationScreenMode.FullScreenWindow => "Borderless Screen",
                ApplicationScreenMode.Windowed => "Windowed",
                _ => ""
            };
        }

        public object GetValue() => _currentModeIndex;
        public bool IsMaxValue() => _currentModeIndex >= System.Enum.GetNames(typeof(ApplicationScreenMode)).Length - 1;
        public bool IsMinValue() => _currentModeIndex == 0;

        public object ToNextValue()
        {
            if (IsMaxValue()) return _currentModeIndex;

            _currentModeIndex++;

            UpdateScreenMode();

            return _currentModeIndex;
        }

        public object ToPreviousValue()
        {
            if (IsMinValue()) return _currentModeIndex;

            _currentModeIndex--;

            UpdateScreenMode();

            return _currentModeIndex;
        }

        public void ResetToDefault()
        {
            _currentModeIndex = _defaultModeIndex;
            _savedModeIndex = _defaultModeIndex;

            UpdateScreenMode();
        }

        public void Save(System.Action<object> onSaved = null)
        {
            _savedModeIndex = _currentModeIndex;

            onSaved?.Invoke(_savedModeIndex);
        }

        public void ResetToSaved()
        {
            _currentModeIndex = _savedModeIndex;

            UpdateScreenMode();
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
