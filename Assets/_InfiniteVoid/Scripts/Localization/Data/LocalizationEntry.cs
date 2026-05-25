namespace Localization
{
    public class LocalizationEntry
    {
        public string english { get; private set; }
        public string russian { get; private set; }

        public LocalizationEntry(string english, string russian)
        {
            this.english = english;
            this.russian = russian;
        }
    }
}
