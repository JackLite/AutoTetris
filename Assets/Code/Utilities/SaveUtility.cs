using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utilities
{
    public static class SaveUtility
    {
        public static void SaveInt(string key, int value, bool saveImmediate = false)
        {
            PlayerPrefs.SetInt(key, value);
            if (saveImmediate)
                PlayerPrefs.Save();
        }

        public static int LoadInt(string key)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : 0;
        }

        public static void SaveBytes(string key, byte[] bytes, bool saveImmediate = false)
        {
            var base64 = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, base64);
            if (saveImmediate)
                PlayerPrefs.Save();
        }

        public static byte[] LoadBytes(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return Convert.FromBase64String(PlayerPrefs.GetString(key));
            return Array.Empty<byte>();
        }

        public static void SaveString(string key, string value, bool saveImmediate = false)
        {
            PlayerPrefs.SetString(key, value);
            if (saveImmediate)
                PlayerPrefs.Save();
        }

        public static string LoadString(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key);
            return string.Empty;
        }
    }
}