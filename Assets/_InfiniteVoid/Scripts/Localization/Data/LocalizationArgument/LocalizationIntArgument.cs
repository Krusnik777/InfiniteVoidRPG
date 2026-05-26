using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LocalizationIntArgument
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public int Value { get; private set; }

        public LocalizationArgument GetLocalizationArgument() => new LocalizationArgument(Key, Value);
    }
}
