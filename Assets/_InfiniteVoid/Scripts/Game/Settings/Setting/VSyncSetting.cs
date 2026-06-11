using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class VSyncSetting : ISetting
    {
        private readonly bool _defaultValue;

        private bool _currentValue;
        private bool _appliedValue;

        public VSyncSetting(bool defaultValue)
        {
            _defaultValue = defaultValue;

            _currentValue = defaultValue;
            _appliedValue = defaultValue;
        }

        public void SetValue(bool state)
        {
            _currentValue = state;

            Apply();
        }

        public string GetNameOfValue() => _currentValue ? "ON" : "OFF";
        public object GetValue() => _currentValue;

        public bool IsMaxValue() => _currentValue == true;
        public bool IsMinValue() => _currentValue == false;
        public bool IsCurrentValueApplied() => _appliedValue == _currentValue;
        public float GetCurrentValueDifference() => _currentValue ? 1 : 0;

        public object ToNextValue(bool applyChanges = true)
        {
            if (IsMaxValue()) return _currentValue;

            _currentValue = true;

            if (applyChanges) Apply();

            return _currentValue;
        }

        public object ToPreviousValue(bool applyChanges = true)
        {
            if (IsMinValue()) return _currentValue;

            _currentValue = false;

            if (applyChanges) Apply();

            return _currentValue;
        }

        public void Apply()
        {
            _appliedValue = _currentValue;

            QualitySettings.vSyncCount = _currentValue ? 1 : 0;
        }

        public void ResetToDefault()
        {
            _currentValue = _defaultValue;

            Apply();
        }
    }
}
