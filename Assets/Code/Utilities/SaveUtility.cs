using UnityEngine;

namespace Utilities
{
    public static class SaveUtility
    {
        public static void SaveInt(string key, int value, bool saveImmediate = true)
        {
            PlayerPrefs.SetInt(key, value);
            if (saveImmediate)
                PlayerPrefs.Save();
        }

        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }
    }
}