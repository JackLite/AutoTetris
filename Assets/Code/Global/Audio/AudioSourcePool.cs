using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Global.Audio
{
    public class AudioSourcePool : IDisposable
    {
        private readonly Transform _parent;
        private readonly Stack<AudioSource> _pool;
        private readonly List<AudioSource> _processed;
        private GameObject _sourcePrefab;

        public AudioSourcePool(int prewarmCount, Transform parent)
        {
            _parent = parent;
            _pool = new Stack<AudioSource>(prewarmCount);
            _processed = new List<AudioSource>(prewarmCount);
            LoadPrefab(prewarmCount);
        }

        public AudioSource GetSource()
        {
            AudioSource s;
            if (_pool.Count == 0)
            {
                s = CreateSource();
                _processed.Add(s);
                return s;
            }
            s = _pool.Pop();
            _processed.Add(s);
            return s;
        }

        public void Check()
        {
            foreach (var source in _processed)
            {
                if (!source.isPlaying)
                    _pool.Push(source);
            }
            _processed.RemoveAll(s => !s.isPlaying);
        }

        public void Dispose()
        {
            Addressables.ReleaseInstance(_sourcePrefab);

            foreach (var source in _pool)
                Object.Destroy(source.gameObject);

            foreach (var source in _processed)
                Object.Destroy(source.gameObject);
        }

        private async void LoadPrefab(int prewarmCount)
        {
            var task = Addressables.LoadAssetAsync<GameObject>("AudioSource").Task;
            await task;
            _sourcePrefab = task.Result;
            for (var i = prewarmCount; i > 0; --i)
                _pool.Push(CreateSource());
        }

        private AudioSource CreateSource()
        {
            if (_sourcePrefab == null)
            {
                Debug.LogError("Source prefab not loaded!");
                return null;
            }
            var instance = Object.Instantiate(_sourcePrefab, _parent);
            return instance.GetComponent<AudioSource>();
        }
    }
}