using UnityEngine;

namespace Core.Grid
{
    public class GridData
    {
        public int Rows;
        public int Columns;
        public bool[,] FillMatrix;
        public bool IsGridStable = true;
        public bool IsNeedCheckPieces;

        public GridData(Vector2Int size)
        {
            Rows = size.x;
            Columns = size.y;
            FillMatrix = new bool[Rows, Columns];
        }
    }
}