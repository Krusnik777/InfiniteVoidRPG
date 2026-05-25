using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Localization
{
    public class LocalizationSystem
    {
        public static Observable<LocalizationLanguage> CurrentLanguage => _instance == null ? CreateInstance().currentLanguage : _instance.currentLanguage;

        private static LocalizationSystem _instance;

        private Dictionary<string, LocalizationTable> tables;

        private ReactiveProperty<LocalizationLanguage> currentLanguage;

        public static LocalizationSystem CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new();

                Init();
            }

            return _instance;
        }

        public static string GetLocalizedString(string tableKey, string key, params KeyValuePair<string, object>[] args)
        {
            if (_instance == null) CreateInstance();

            var table = _instance.tables[tableKey];

            return table.Get(key, _instance.currentLanguage.Value, args);
        }

        public static void ChangeLanguage(LocalizationLanguage language)
        {
            if (_instance == null) CreateInstance();

            _instance.currentLanguage.Value = language;
        }

        private static void Init()
        {
            var systemLang = Application.systemLanguage;

            _instance.currentLanguage = new(systemLang switch
            {
                SystemLanguage.English => LocalizationLanguage.English,
                SystemLanguage.Russian => LocalizationLanguage.Russian,
                _ => LocalizationLanguage.English
            });

            CreateLocalizationTables();
        }

        private static void CreateLocalizationTables()
        {
            _instance.tables = new();

            foreach (var tableName in System.Enum.GetNames(typeof(LocalizationTables)))
            {
                var table = new LocalizationTable(tableName);

                if (table != null) _instance.tables.Add(tableName, table);
            }
        }
    }
}
