using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Global.Settings.Localization.Editor
{
    public static class LocalizationParser
    {
        private const char COLUMN_SEPARATOR = ';';
        public static Dictionary<string, TextAsset> Parse(string csv)
        {
            using var reader = new StringReader(csv);
            var firstRow = reader.ReadLine();
            var languages = GetLanguages(firstRow);
            var langToAssetMap = new Dictionary<string, TextAsset>();
            var langToCsvMap = new Dictionary<string, string>();
            string row;
            while((row = reader.ReadLine()) != null)
            {
                var values = row.Split(COLUMN_SEPARATOR);
                for (var i = 0; i < languages.Length; i++)
                {
                    var key = values[0];
                    if (i >= values.Length)
                    {
                        Debug.LogError($"Missing value for {key}");
                        continue;
                    }
                    var val = values[i + 1];
                    var lang = languages[i];
                    if (!langToCsvMap.ContainsKey(lang))
                        langToCsvMap[lang] = string.Empty;

                    langToCsvMap[lang] += $"{key};{val}\r\n";
                }
            }
            foreach (var kvp in langToCsvMap)
            {
                langToAssetMap[kvp.Key] = new TextAsset(kvp.Value.Substring(0, kvp.Value.Length - 2));
            }

            return langToAssetMap;
        }

        private static string[] GetLanguages(string firstRow)
        {
            var cols = firstRow.Split(COLUMN_SEPARATOR);
            var result = new string[cols.Length - 1];
            Array.Copy(cols, 1, result, 0, result.Length);
            return result;
        }
    }
}