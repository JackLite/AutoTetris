using UnityEngine;

namespace Core.Figures
{
    public struct FigureComponent
    {
        public FigureType Type;

        /// <summary>
        /// Позиция нижнего левой нижней точки в координатах сетки независимо от поворота
        /// </summary>
        public int Row;

        public int Column;

        public FigureRotation Rotation;

        public FigureMono Mono;

        public bool IsMoving;
    }
}