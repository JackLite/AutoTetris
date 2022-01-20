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

        public static int GetInt(string key)
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

        public static byte[] GetBytes(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return Convert.FromBase64String(PlayerPrefs.GetString(key));
            return Array.Empty<byte>();
        }
    }
}