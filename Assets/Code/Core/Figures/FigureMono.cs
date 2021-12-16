using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Figures
{
    [RequireComponent(typeof(RectTransform))]
    public class FigureMono : MonoBehaviour
    {
        public const int CELL_SIZE = 75;

        [SerializeField]
        private RotationStructure[] rotations;

        private RectTransform _rect;
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void Fall()
        {
            _rect.anchoredPosition += Vector2.down * CELL_SIZE;
        }

        /// <summary>
        /// Выставляет позицию на сетке
        /// Отсчёт идёт от левого нижнего угла сетки
        /// </summary>
        public void SetGridPosition(int row, int column)
        {
            _rect.anchoredPosition = new Vector2(column * CELL_SIZE, row * CELL_SIZE);
        }

        public void Delete()
        {
            Addressables.ReleaseInstance(gameObject);
        }

        public void Rotate(FigureRotation rotation)
        {
            foreach (var structure in rotations)
            {
                structure.rotationObject.SetActive(structure.rotations.Contains(rotation));
            }
        }

        [Serializable]
        private struct RotationStructure
        {
            public GameObject rotationObject;
            public FigureRotation[] rotations;
        }
    }
}