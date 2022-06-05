using System;
using System.Collections.Generic;
using System.Linq;
using Core.Figures;
using UnityEngine;
using Utilities;

namespace Core.Cells.Visual
{
    public class ShadowFigure : MonoBehaviour
    {
        [field:SerializeField]
        public FigureType FigureType { get; private set; }

        [SerializeField]
        private RotationToGo[] _rotationToGo = Array.Empty<RotationToGo>();

        private Lazy<Dictionary<FigureRotation, GameObject>> Map =>
            new(() => _rotationToGo.CreateMap(d => d.rotation, d => d.gameObject));

        public void Show(FigureRotation rotation)
        {
            foreach (var rotationToGo in _rotationToGo)
                rotationToGo.gameObject.SetActive(rotationToGo.rotation == rotation);
        }

        public GameObject GetFigureGo(FigureRotation rotation)
        {
            return Map.Value[rotation];
        }

        [Serializable]
        private struct RotationToGo
        {
            public FigureRotation rotation;
            public GameObject gameObject;
        }
    }
}