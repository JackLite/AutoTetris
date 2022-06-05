using System;
using System.Collections.Generic;
using Core.Figures;
using UnityEngine;
using Utilities;

namespace Core.Cells.Visual
{
    [RequireComponent(typeof(RectTransform))]
    public class ShadowCells : MonoBehaviour
    {
        [SerializeField]
        private ShadowCellCreator _shadowCellCreator;

        [SerializeField]
        private ShadowArrowController _shadowArrowController;

        [SerializeField]
        private FigureToGo[] _figureToGo = Array.Empty<FigureToGo>();

        private Lazy<Dictionary<FigureType, ShadowFigure>> Map =>
            new(() => _figureToGo.CreateMap(d => d.figureType, d => d.shadowFigure));


        private Lazy<RectTransform> Rect => new(GetComponent<RectTransform>);

        public void Init(Direction direction)
        {
            _shadowCellCreator.CreateCells(direction);
        }

        public void Show(in Figure figure, Direction direction)
        {
            _shadowCellCreator.ReInitCells(direction);
            _shadowCellCreator.ShowCells();
            var shadowFigure = Map.Value[figure.type];
            var cellsParent = shadowFigure.GetFigureGo(figure.rotation).transform;
            _shadowCellCreator.SetParent(cellsParent);
            shadowFigure.Show(figure.rotation);
            
            var pos = new Vector2(figure.column * FigureMono.CELL_SIZE, figure.row * FigureMono.CELL_SIZE);
            Rect.Value.anchoredPosition = pos;
            _shadowArrowController.direction = direction;
        }

        [Serializable]
        private struct FigureToGo
        {
            public FigureType figureType;
            public ShadowFigure shadowFigure;
        }

        public void Hide()
        {
            _shadowCellCreator.HideCells();
        }
    }
}