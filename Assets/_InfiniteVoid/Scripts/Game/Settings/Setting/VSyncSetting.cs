using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class VSyncSetting : ISetting
    {
        private readonly bool _defaultValue;

        private bool _currentValue;
        private bool _savedValue;


        public VSyncSetting(bool defaultValue)
        {
            _defaultValue = defaultValue;

            _currentValue = defaultValue;
            _savedValue = defaultValue;
        }

        public void SetValue(bool state)
        {
            _currentValue = state;
            _savedValue = state;

            UpdateVSync();
        }

        public string GetNameOfValue() => _currentValue ? "ON" : "OFF";

        public object GetValue() => _currentValue;

        public bool IsMaxValue() => _currentValue == true;

        public bool IsMinValue() => _currentValue == false;

        public object ToNextValue()
        {
            if (IsMaxValue()) return _currentValue;

            _currentValue = true;

            UpdateVSync();

            return _currentValue;
        }

        public object ToPreviousValue()
        {
            if (IsMinValue()) return _currentValue;

            _currentValue = false;

            UpdateVSync();

            return _currentValue;
        }

        public void ResetToDefault()
        {
            _currentValue = _defaultValue;
            _savedValue = _defaultValue;

            UpdateVSync();
        }

        public void Save(System.Action<object> onSaved = null)
        {
            _savedValue = _currentValue;

            onSaved?.Invoke(_savedValue);
        }
        public void ResetToSaved()
        {
            _currentValue = _savedValue;

            UpdateVSync();
        }

        private void UpdateVSync()
        {
            QualitySettings.vSyncCount = _currentValue ? 1 : 0;
        }
    }
}
