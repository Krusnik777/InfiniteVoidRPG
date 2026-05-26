using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LocalizationStringArgument
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public string Value { get; private set; }

        public LocalizationArgument GetLocalizationArgument() => new LocalizationArgument(Key, Value);
    
    }
}
