using Core.Figures;
using UnityEngine;

namespace Core.Cells.Visual
{
    public class ShadowArrowController : MonoBehaviour
    {
        [SerializeField]
        private ShadowCellCreator _creator;

        [SerializeField]
        private CellConfig _cellConfig;

        public Direction direction = Direction.None;

        private void Update()
        {
            if (direction == Direction.None || !_creator.IsInit)
                return;
            for (var x = 0; x < _creator.Cells.GetLength(0); ++x)
            {
                for (var y = 0; y < _creator.Cells.GetLength(1); ++y)
                {
                    var cell = _creator.Cells[x, y];
                    var oldPos = cell.Position;
                    if (IsNeedToReset(oldPos))
                    {
                        cell.SetPosition(GetStartPos(oldPos));
                        continue;
                    }
                    cell.SetPosition(GetNewPos(oldPos));
                }
            }
        }
        private Vector2 GetNewPos(in Vector2 oldPos)
        {
            var delta = _cellConfig.MoveSpeed * Time.deltaTime;
            return direction switch
            {
                Direction.Right  => new Vector2(oldPos.x + delta, oldPos.y),
                Direction.Left   => new Vector2(oldPos.x - delta, oldPos.y),
                Direction.Bottom => new Vector2(oldPos.x, oldPos.y - delta),
                _                => Vector2.zero
            };
        }
        private Vector2 GetStartPos(in Vector2 oldPos)
        {
            return direction switch
            {
                Direction.Right  => new Vector2(-_cellConfig.Distance, oldPos.y),
                Direction.Left   => new Vector2(_cellConfig.Distance * 5, oldPos.y),
                Direction.Bottom => new Vector2(oldPos.x, _cellConfig.Distance * 5),
                _                => Vector2.zero
            };
        }
        private bool IsNeedToReset(in Vector2 oldPos)
        {
            return direction switch
            {
                Direction.Right  => oldPos.x >= _cellConfig.Distance * 5,
                Direction.Left   => oldPos.x <= -_cellConfig.Distance,
                Direction.Bottom => oldPos.y <= -_cellConfig.Distance,
                _                => false
            };
        }
    }
}