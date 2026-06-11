using UnityEngine.Audio;

namespace InfiniteVoidRPG.Game.Settings
{
    public class SoundSetting : ISetting
    {
        private const int _minValue = -80;
        private const int _maxValue = 20;
        private const int _step = 5;

        private readonly string _soundGroupName;
        private readonly AudioMixer _audioMixer;
        private readonly int _defaultValue;

        private int _currentValue;
        private int _appliedValue;

        public SoundSetting(string soundGroupName, AudioMixer audioMixer, int defaultValue)
        {
            _soundGroupName = soundGroupName;
            _audioMixer = audioMixer;
            _defaultValue = defaultValue;

            _currentValue = defaultValue;
            _appliedValue = defaultValue;
        }

        public void SetValue(int value)
        {
            _currentValue = value;

            Apply();
        }

        public string GetNameOfValue() => _currentValue.ToString();
        public object GetValue() => _currentValue;

        public bool IsMaxValue() => _currentValue >= _maxValue;
        public bool IsMinValue() => _currentValue <= _minValue;
        public bool IsCurrentValueApplied() => _appliedValue == _currentValue;
        public float GetCurrentValueDifference() => (float)_currentValue / (float)_maxValue;

        public object ToNextValue(bool applyChanges = true)
        {
            if (IsMaxValue()) return _currentValue;

            _currentValue += _step;

            if (_currentValue > _maxValue) _currentValue = _maxValue;

            if (applyChanges) Apply();

            return _currentValue;
        }

        public object ToPreviousValue(bool applyChanges = true)
        {
            if (IsMinValue()) return _currentValue;

            _currentValue -= _step;

            if (_currentValue < _minValue) _currentValue = _minValue;

            if (applyChanges) Apply();

            return _currentValue;
        }

        public void Apply()
        {
            _appliedValue = _currentValue;
            
            _audioMixer.SetFloat(_soundGroupName, _currentValue);
        }

        public void ResetToDefault()
        {
            _currentValue = _defaultValue;

            Apply();
        }
    }
}
