using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Localization
{
    public class LocalizationTable
    {
        public string TableName { get; private set; }

        private const string _localizationsTablesPath = "Localization";

        private Dictionary<string, LocalizationEntry> _entries;

        public LocalizationTable(string tableName)
        {
            TableName = tableName;

            var path = Path.Combine(_localizationsTablesPath, tableName);
            var tableAsset = Resources.Load<TextAsset>(path);

            if (tableAsset == null) throw new System.ArgumentNullException($"Not found table file in Resources for name: {tableName}");

            _entries = CSVParser.LoadLocalizedStringsFromCSV(tableAsset);
        }

        public string Get(string key, LocalizationLanguage language, params KeyValuePair<string, object>[] args)
        {
            if (!_entries.ContainsKey(key)) throw new System.ArgumentNullException($"Not found strings for key: {key}");

            var entry = _entries[key];

            var text = language switch
            {
                LocalizationLanguage.English => entry.english,
                LocalizationLanguage.Russian => entry.russian,
                _ => entry.english
            };

            return args.Length > 0 ? StringFormatter.Format(text, language, args.ToDictionary(pair => pair.Key, pair => pair.Value)) : text;
        }
    }
}
