using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public static class CSVParser
    {
        private const char _lineSeparator = '\n';
        private const char _rowsSeparator = ';';

        public static Dictionary<string, LocalizationEntry> LoadLocalizedStringsFromCSV(TextAsset csvFile)
        {
            Dictionary<string, LocalizationEntry> dict = new();
            var lines = csvFile.text.Split(_lineSeparator);
            
            //var headers = lines[0].Split(_rowsSeparator);
            //headers = ["Key", "English", "Russian"]

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                var values = lines[i].Split(_rowsSeparator);

                if (values.Length == 0) continue;

                var entry = new LocalizationEntry(values[1], values[2]);
                dict.Add(values[0], entry);
            }

            return dict;
        }
    }
}
