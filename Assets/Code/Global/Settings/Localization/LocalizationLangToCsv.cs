using System;
using UnityEngine;

namespace Global.Settings.Localization
{
    [Serializable]
    public struct LocalizationLangToCsv
    {
        public Languages lang;
        public TextAsset csv;
    }
}