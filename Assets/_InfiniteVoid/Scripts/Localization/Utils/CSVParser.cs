using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public static class CSVParser
    {
        public static Dictionary<string, LocalizationEntry> LoadLocalizedStringsFromCSV(TextAsset csvFile)
        {
            Dictionary<string, LocalizationEntry> dict = new();
            var lines = csvFile.text.Split('\n');
            
            //var headers = lines[0].Split(';');
            //headers = ["Key", "English", "Russian"]

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;

                var values = lines[i].Split(';');

                if (values.Length == 0) continue;

                var entry = new LocalizationEntry(values[1], values[2]);
                dict.Add(values[0], entry);
            }

            return dict;
        }
    }
}
