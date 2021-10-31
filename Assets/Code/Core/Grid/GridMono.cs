using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Grid
{
    public class GridMono : MonoBehaviour
    {
        private GameObject _hint;
        public async void LightUp(GridPosition cellPosition)
        {
            var position = new Vector2(cellPosition.Column * 85, cellPosition.Row * 85);
            var op = Addressables.InstantiateAsync("FigureHint_O",  transform);
            await op.Task;
            _hint = op.Result;
            _hint.GetComponent<RectTransform>().anchoredPosition = position;
        }

        public void LightDown()
        {
            _hint.SetActive(false);
            Addressables.ReleaseInstance(_hint);
        }
    }
}