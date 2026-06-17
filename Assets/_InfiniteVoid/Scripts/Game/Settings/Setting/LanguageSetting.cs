using Localization;

namespace InfiniteVoidRPG.Game.Settings
{
    public class LanguageSetting : ISetting
    {
        private readonly int _defaultLanguageIndex;

        private int _currentLanguageIndex;
        private int _appliedLanguageIndex;

        public LanguageSetting(int defaultLanguageIndex)
        {
            _defaultLanguageIndex = defaultLanguageIndex;

            _currentLanguageIndex = _defaultLanguageIndex;
            _appliedLanguageIndex = _defaultLanguageIndex;
        }

        public void SetValue(int index)
        {
            _currentLanguageIndex = index;

            Apply();
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
        public bool IsCurrentValueApplied() => _appliedLanguageIndex == _currentLanguageIndex;
        public float GetCurrentValueDifference() => (float)_currentLanguageIndex/(float)(System.Enum.GetNames(typeof(LocalizationLanguage)).Length - 1);

        public object ToNextValue(bool applyChanges = true)
        {
            if (IsMaxValue()) return _currentLanguageIndex;

            _currentLanguageIndex++;

            if (applyChanges) Apply();

            return _currentLanguageIndex;
        }

        public object ToPreviousValue(bool applyChanges = true)
        {
            if (IsMinValue()) return _currentLanguageIndex;

            _currentLanguageIndex--;

            if (applyChanges) Apply();

            return _currentLanguageIndex;
        }

        public void Apply()
        {
            _appliedLanguageIndex = _currentLanguageIndex;

            LocalizationSystem.ChangeLanguage((LocalizationLanguage)_currentLanguageIndex);
        }

        public void ResetToApplied(bool applyChanges = false)
        {
            _currentLanguageIndex = _appliedLanguageIndex;

            if (applyChanges) Apply();
        }

        public void ResetToDefault()
        {
            _currentLanguageIndex = _defaultLanguageIndex;

            Apply();
        }
    }
}
