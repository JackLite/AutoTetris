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
                Addressables.ReleaseInstance(_current);

            _current = await Addressables.InstantiateAsync(FiguresUtility.GetFigureAddress(type), _rect).Task;
        }
    }
}