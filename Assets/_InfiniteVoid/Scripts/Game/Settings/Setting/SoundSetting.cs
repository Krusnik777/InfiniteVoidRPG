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
        private int _savedValue;


        public SoundSetting(string soundGroupName, AudioMixer audioMixer, int defaultValue)
        {
            _soundGroupName = soundGroupName;
            _audioMixer = audioMixer;
            _defaultValue = defaultValue;

            _currentValue = defaultValue;
            _savedValue = defaultValue;
        }

        public void SetValue(int value)
        {
            _currentValue = value;
            _savedValue = _currentValue;

            _audioMixer.SetFloat(_soundGroupName, _currentValue);
        }

        public string GetNameOfValue() => _currentValue.ToString();
        public object GetValue() => _currentValue;

        public bool IsMaxValue() => _currentValue >= _maxValue;
        public bool IsMinValue() => _currentValue <= _minValue;

        public object ToNextValue()
        {
            if (IsMaxValue()) return _currentValue;

            _currentValue += _step;

            if (_currentValue > _maxValue) _currentValue = _maxValue;

            _audioMixer.SetFloat(_soundGroupName, _currentValue);

            return _currentValue;
        }

        public object ToPreviousValue()
        {
            if (IsMinValue()) return _currentValue;

            _currentValue -= _step;

            if (_currentValue < _minValue) _currentValue = _minValue;

            _audioMixer.SetFloat(_soundGroupName, _currentValue);

            return _currentValue;
        }

        public void ResetToDefault()
        {
            _currentValue = _defaultValue;
            _savedValue = _defaultValue;

            _audioMixer.SetFloat(_soundGroupName, _currentValue);
        }

        public void Save(System.Action<object> onSaved = null)
        {
            _savedValue = _currentValue;

            onSaved?.Invoke(_savedValue);
        }

        public void ResetToSaved()
        {
            _currentValue = _savedValue;

            _audioMixer.SetFloat(_soundGroupName, _currentValue);
        }
    }
}
