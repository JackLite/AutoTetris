using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Global.Settings.Localization.Editor
{
    public static class LocalizationSettingEditorParser
    {
        public static void Parse(SerializedProperty property)
        {
            var rawProperty = property.FindPropertyRelative("rawLocalizationJson");
            var asset = rawProperty.objectReferenceValue as TextAsset;
            if (asset == null)
                Debug.LogError("Wrong raw localization");
            else
                CreateAssets(property, asset);
        }

        private static void CreateAssets(SerializedProperty property, TextAsset rawLocalization)
        {
            var localizationsProperty = property.FindPropertyRelative("localizations");

            var langToTextAsset = LocalizationParser.Parse(rawLocalization.text);
            var i = 0;
            localizationsProperty.ClearArray();
            var langMap = GetLangMap(property);
            foreach (var kvp in langToTextAsset) 
            {
                var path = AssetDatabase.GetAssetPath(rawLocalization);
                var indexOf = path.LastIndexOf(rawLocalization.name, StringComparison.InvariantCulture);
                var directory = path.Substring(0, indexOf);
                var newAssetPath = $"{directory}Localization_{kvp.Key}.csv";
                if (!File.Exists(newAssetPath))
                    using(File.Create(newAssetPath))
                    {
                    }
                File.WriteAllText(newAssetPath, kvp.Value);
                AssetDatabase.Refresh();
                localizationsProperty.InsertArrayElementAtIndex(i);
                var subProp = localizationsProperty.GetArrayElementAtIndex(i);
                subProp.FindPropertyRelative("lang").enumValueIndex = langMap[kvp.Key];
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(newAssetPath);
                EditorUtility.SetDirty(asset);
                subProp.FindPropertyRelative("csv").objectReferenceValue = asset;
                i++;
            }
        }


        private static Dictionary<string, int> GetLangMap(SerializedProperty property)
        {
            var mapProperty = property.FindPropertyRelative("langKeyToEnumMap");
            var map = new Dictionary<string, int>();
            for (var i = 0; i < mapProperty.arraySize; ++i)
            {
                var langToEnumProp = mapProperty.GetArrayElementAtIndex(i);
                var keyProp = langToEnumProp.FindPropertyRelative("key");
                var enumProp = langToEnumProp.FindPropertyRelative("lang");
                map.Add(keyProp.stringValue, enumProp.enumValueIndex);
            }
            return map;
        }
    }
}