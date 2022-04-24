using System;
using UnityEngine;

namespace Global.Settings.Localization
{
    [Serializable]
    public class LocalizationSettings
    {
        public TextAsset rawLocalizationJson;
        public LangKeyToLangEnum[] langKeyToEnumMap;
        public LocalizationLangToCsv[] localizations;

        [Serializable]
        public class LangKeyToLangEnum
        {
            public string key;
            public Languages lang;
        }
    }
}