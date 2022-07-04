using System.Collections.Generic;
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

        private readonly Dictionary<ShadowCells, Transform> _defaultParents = new();

        public void Init()
        {
            _leftShadow.Init(Direction.Left);
            _rightShadow.Init(Direction.Right);
            _bottomShadow.Init(Direction.Bottom);

            _defaultParents[_bottomShadow] = _bottomShadow.transform.parent;
            _defaultParents[_rightShadow] = _rightShadow.transform.parent;
            _defaultParents[_leftShadow] = _leftShadow.transform.parent;
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

        public void LightUpRight()
        {
            LightUpShadow(_rightShadow);
        }
        
        public void LightUpLeft()
        {
            LightUpShadow(_leftShadow);
        }
        
        public void LightUpBottom()
        {
            LightUpShadow(_bottomShadow);
        }

        private static void LightUpShadow(Component shadowCells)
        {
            var canvas = shadowCells.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }

        public void ResetRightParent()
        {
            Destroy(_rightShadow.gameObject.GetComponent<Canvas>());
        }
        
        public void ResetLeftParent()
        {
            Destroy(_leftShadow.gameObject.GetComponent<Canvas>());
        }
        
        public void ResetBottomParent()
        {
            Destroy(_bottomShadow.gameObject.GetComponent<Canvas>());
        }
    }
}