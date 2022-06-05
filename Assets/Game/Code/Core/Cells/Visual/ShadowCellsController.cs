using Core.Figures;
using UnityEngine;

namespace Core.Cells.Visual
{
    public class ShadowCellsController : MonoBehaviour
    {
        [SerializeField]
        private ShadowCells _leftShadow;

        [SerializeField]
        private ShadowCells _rightShadow;

        [SerializeField]
        private ShadowCells _bottomShadow;

        public void Init()
        {
            _leftShadow.Init(Direction.Left);
            _rightShadow.Init(Direction.Right);
            _bottomShadow.Init(Direction.Bottom);
        }

        public void ShowLeft(in Figure figure)
        {
            _leftShadow.Show(figure, Direction.Left);
        }

        public void ShowRight(in Figure figure)
        {
            _rightShadow.Show(figure, Direction.Right);
        }

        public void ShowBottom(in Figure figure)
        {
            _bottomShadow.Show(figure, Direction.Bottom);
        }

        public void HideAll()
        {
            HideLeft();
            HideRight();
            HideBottom();
        }

        public void Hide(Direction decisionDirection)
        {
            switch (decisionDirection)
            {
                case Direction.Left:
                    HideLeft();
                    break;
                case Direction.Right:
                    HideRight();
                    break;
                case Direction.Bottom:
                    HideBottom();
                    break;
            }
        }

        private void HideLeft()
        {
            _leftShadow.Hide();
        }

        private void HideRight()
        {
            _rightShadow.Hide();
        }

        private void HideBottom()
        {
            _bottomShadow.Hide();
        }
    }
}