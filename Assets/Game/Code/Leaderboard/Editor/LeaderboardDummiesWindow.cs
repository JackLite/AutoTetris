using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leaderboard.Editor
{
    public class LeaderboardDummiesWindow : EditorWindow
    {
        private static TextAsset _rawJson;
        private static string _resultFileName;
        private static readonly Dictionary<string, bool> _regions = new Dictionary<string, bool>();
        private Vector2 _scrollPos = Vector2.zero;
        private static int _resultCount = 3476;

        [MenuItem("AWC/Leaderboard Dummies")]
        private static void ShowWindow()
        {
            var window = GetWindow<LeaderboardDummiesWindow>();
            window.titleContent = new GUIContent("Leaderboard Dummies");
            window.Show();
        }

        private void OnGUI()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos,
                false,
                true,
                GUILayout.MinWidth(400),
                GUILayout.MinHeight(400));
            DrawBase();
            if (_rawJson == null)
            {
                GUILayout.EndScrollView();
                return;
            }

            var jToken = JArray.Parse(_rawJson.text);
            var regions = new List<NamesRegion>(jToken.Count);
            foreach (var t in jToken)
            {
                regions.Add(t.ToObject<NamesRegion>());
            }

            DrawRegions(regions);
            _resultCount = EditorGUILayout.IntField("Result counts", _resultCount);

            if (GUILayout.Button("Generate"))
            {
                CreateResultJson(regions);
            }

            GUILayout.EndScrollView();
        }

        private static void CreateResultJson(IReadOnlyList<NamesRegion> regions)
        {
            var resultNames = new List<string>();
            var regionIndex = 0;
            while (resultNames.Count < _resultCount)
            {
                var region = regions[regionIndex];
                if (!_regions.ContainsKey(region.region) || !_regions[region.region])
                {
                    ++regionIndex;
                    if (regionIndex >= regions.Count)
                        regionIndex = 0;
                    continue;
                }
                var names = Random.Range(0, 1f) > .5f ? region.female : region.male;
                var resName = names[Random.Range(0, names.Length)];
                resultNames.Add(resName);
                ++regionIndex;
                if (regionIndex >= regions.Count)
                    regionIndex = 0;
            }

            var arr = new JArray();
            foreach (var resultName in resultNames)
            {
                var o = new JObject
                {
                    ["scores"] = Random.Range(0, 100000),
                    ["name"] = resultName
                };
                arr.Add(o);
            }
            if (!File.Exists(_resultFileName))
                using (File.Create(_resultFileName))
                {
                }
            File.WriteAllText(_resultFileName, arr.ToString());
            AssetDatabase.Refresh();
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(_resultFileName);
            EditorUtility.SetDirty(asset);
        }

        private static void DrawRegions(List<NamesRegion> regions)
        {
            foreach (var r in regions)
            {
                if (!_regions.ContainsKey(r.region))
                    _regions[r.region] = false;
                _regions[r.region] = EditorGUILayout.ToggleLeft(r.region, _regions[r.region], GUILayout.MinHeight(20));
            }
        }

        private static void DrawBase()
        {
            _rawJson = (TextAsset) EditorGUILayout.ObjectField("Raw json", _rawJson, typeof(TextAsset), false);
            _resultFileName = EditorGUILayout.TextField("Result Filename", _resultFileName);
        }

        [Serializable]
        public class NamesRegion
        {
            public string region;
            public string[] male;
            public string[] female;
            public string[] surnames;
        }
    }
}