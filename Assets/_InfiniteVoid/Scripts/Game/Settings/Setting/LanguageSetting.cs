using Localization;

namespace InfiniteVoidRPG.Game.Settings
{
    public class LanguageSetting : ISetting
    {
        private readonly int _defaultLanguageIndex;

        private int _currentLanguageIndex;
        private int _savedLanguageIndex;

        public LanguageSetting(int defaultLanguageIndex)
        {
            _defaultLanguageIndex = defaultLanguageIndex;

            _currentLanguageIndex = _defaultLanguageIndex;
            _savedLanguageIndex = _defaultLanguageIndex;
        }

        public void SetValue(int index)
        {
            _currentLanguageIndex = index;

            UpdateLanguage();
        }

        public string GetNameOfValue()
        {
            LocalizationLanguage language = (LocalizationLanguage) _currentLanguageIndex;

            return language switch
            {
                LocalizationLanguage.English => "English",
                LocalizationLanguage.Russian => "Русский",
                _ => ""
            };
        }

        public object GetValue() => _currentLanguageIndex;

        public bool IsMaxValue() => _currentLanguageIndex >= System.Enum.GetNames(typeof(LocalizationLanguage)).Length - 1;

        public bool IsMinValue() => _currentLanguageIndex == 0;

        public object ToNextValue()
        {
            if (IsMaxValue()) return _currentLanguageIndex;

            _currentLanguageIndex++;

            UpdateLanguage();

            return _currentLanguageIndex;
        }

        public object ToPreviousValue()
        {
            if (IsMinValue()) return _currentLanguageIndex;

            _currentLanguageIndex--;

            UpdateLanguage();

            return _currentLanguageIndex;
        }

        public void ResetToDefault()
        {
            _currentLanguageIndex = _defaultLanguageIndex;
            _savedLanguageIndex = _defaultLanguageIndex;

            UpdateLanguage();
        }

        public void Save(System.Action<object> onSaved = null)
        {
            _savedLanguageIndex = _currentLanguageIndex;

            onSaved?.Invoke(_savedLanguageIndex);
        }

        public void ResetToSaved()
        {
            _currentLanguageIndex = _savedLanguageIndex;

            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            LocalizationSystem.ChangeLanguage((LocalizationLanguage)_currentLanguageIndex);
        }
    }
}
