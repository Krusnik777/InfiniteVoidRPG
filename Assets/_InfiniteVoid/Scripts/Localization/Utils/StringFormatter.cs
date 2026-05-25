using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Localization
{
    public static class StringFormatter
    {
        private static readonly Regex _placeholderRegex = new Regex(@"\{(\w+)(?::([^}]+))?\}");

        private const string _functionCommandEnd = ")";
        private const string _pluralFunctionCommand = "plural(";
        private const string _selectFunctionCommand = "select(";

        public static string Format(string targetText, LocalizationLanguage language, Dictionary<string, object> args)
        {
            return _placeholderRegex.Replace(targetText, match =>
            {
                string key = match.Groups[1].Value;

                if (!args.ContainsKey(key))
                {
                    UnityEngine.Debug.LogWarning($"Localization.StringFormatter: Key '{key}' not found in arguments.");
                    return match.Value;
                }

                string functionPart = match.Groups[2].Success ? match.Groups[2].Value : null;
                object rawValue = args[key];

                if (string.IsNullOrEmpty(functionPart)) return rawValue.ToString();

                return ApplyFunction(key, language, rawValue, functionPart);
            });
        }

        private static string ApplyFunction(string key, LocalizationLanguage language, object rawValue, string functionCall)
        {
            if (functionCall.StartsWith(_pluralFunctionCommand) && functionCall.EndsWith(_functionCommandEnd))
            {
                string argsList = functionCall.Substring(_pluralFunctionCommand.Length, functionCall.Length - (_pluralFunctionCommand.Length + _functionCommandEnd.Length));
                string[] forms = SplitArguments(argsList);

                if (!IsNumeric(rawValue))
                {
                    UnityEngine.Debug.LogWarning($"Localization.StringFormatter: plural function requires numeric value for key '{key}', got {rawValue?.GetType()}");
                    return rawValue.ToString();
                }

                int number = ConvertToInt(rawValue);
                int pluralFormIndex = GetPluralFormIndex(number, language);
                if (pluralFormIndex >= forms.Length) pluralFormIndex = forms.Length - 1;

                return forms[pluralFormIndex];
            }

            if (functionCall.StartsWith(_selectFunctionCommand) && functionCall.EndsWith(_functionCommandEnd))
            {
                string argsList = functionCall.Substring(_selectFunctionCommand.Length, functionCall.Length - (_selectFunctionCommand.Length + _functionCommandEnd.Length));
                var pairs = ParseSelectPairs(argsList);

                string stringValue = rawValue.ToString();
                if (pairs.TryGetValue(stringValue, out string selected))
                    return selected;
                else if (pairs.TryGetValue("_default", out string defaultSelected))
                    return defaultSelected;
                else
                    return rawValue.ToString();
            }

            UnityEngine.Debug.LogWarning($"Localization.StringFormatter: Unknown function '{functionCall}' for key '{key}'. Using raw value.");
            return rawValue.ToString();
        }

        #region Plural() Function

        private static string[] SplitArguments(string argsList)
        {
            var parts = argsList.Split('|');

            for (int i = 0; i < parts.Length; i++)
                parts[i] = parts[i].Trim();

            return parts;
        }

        private static bool IsNumeric(object value)
        {
            return value is sbyte || value is byte || value is short || value is ushort ||
                   value is int || value is uint || value is long || value is ulong ||
                   value is float || value is double || value is decimal;
        }

        private static int ConvertToInt(object value) => System.Convert.ToInt32(value);
        private static int GetPluralFormIndex(int number, LocalizationLanguage language)
        {
            if (language == LocalizationLanguage.Russian)
            {
                int mod10 = number % 10;
                int mod100 = number % 100;
                if (mod10 == 1 && mod100 != 11) return 0;
                if (mod10 >= 2 && mod10 <= 4 && (mod100 < 10 || mod100 >= 20)) return 1;
                return 2;
            }
            else if (language == LocalizationLanguage.English)
            {
                return number == 1 ? 0 : 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Select() Function

        private static Dictionary<string, string> ParseSelectPairs(string argsList)
        {
            var dict = new Dictionary<string, string>();
            var parts = argsList.Split('|');

            foreach (var part in parts)
            {
                var kv = part.Split('=');
                if (kv.Length == 2)
                    dict[kv[0].Trim()] = kv[1].Trim();
            }

            if (!dict.ContainsKey("_default"))
                dict["_default"] = dict.Values.FirstOrDefault() ?? "";

            return dict;
        }

        #endregion
    }
}
