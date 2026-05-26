namespace Localization
{
    public class LocalizationArgument
    {
        public string Key { get; private set; }
        public object Value { get; private set; }

        public LocalizationArgument(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
