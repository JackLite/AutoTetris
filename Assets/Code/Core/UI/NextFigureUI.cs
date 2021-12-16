using System;
using Core.Figures;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class NextFigureUI : MonoBehaviour
    {
        private GameObject _current;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public async void ShowNext(FigureType type)
        {
            if (_current != null)
                Destroy(_current);

            var task = Addressables.InstantiateAsync(FiguresUtility.GetFigureAddress(type), _rect).Task;
            await task;
            _current = task.Result;
        }
    }
}