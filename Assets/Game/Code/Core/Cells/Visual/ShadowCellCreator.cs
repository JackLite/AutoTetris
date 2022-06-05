using Core.Figures;
using UnityEngine;

namespace Core.Cells.Visual
{
    public class ShadowCellCreator : MonoBehaviour
    {
        [SerializeField]
        private ShadowCell _shadowCell;

        [SerializeField]
        private CellConfig _cellConfig;

        public ShadowCell[,] Cells { get; set; } = new ShadowCell[6, 6];

        public bool IsInit => Cells[5, 5] != null;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void CreateCells(Direction direction)
        {
            for (var x = 0; x < Cells.GetLength(0); ++x)
            {
                for (var y = 0; y < Cells.GetLength(1); ++y)
                {
                    var pos = GetStartPos(direction, x, y);
                    var inst = Instantiate(_shadowCell, transform);
                    inst.Init(pos, direction);
                    Cells[x, y] = inst;
                }
            }
        }
        private Vector2 GetStartPos(Direction direction, int x, int y)
        {
            return direction switch
            {
                Direction.Right  => new Vector2((x - 1) * _cellConfig.Distance, (y - 1) * FigureMono.CELL_SIZE),
                Direction.Left   => new Vector2((x - 1) * _cellConfig.Distance, (y - 1) * FigureMono.CELL_SIZE),
                Direction.Bottom => new Vector2((x - 1) * FigureMono.CELL_SIZE, (y - 1) * _cellConfig.Distance),
                _                => Vector2.zero
            };
        }

        public void ReInitCells(Direction direction)
        {
            for (var x = 0; x < Cells.GetLength(0); ++x)
            {
                for (var y = 0; y < Cells.GetLength(1); ++y)
                {
                    var pos = GetStartPos(direction, x, y);
                    Cells[x, y].Init(pos, direction);
                }
            }
        }
    }
}