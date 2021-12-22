namespace Core.Grid
{
    public class GridData
    {
        public int Rows;
        public int Columns;
        public bool[,] FillMatrix;
        public bool IsGridStable = true;
        public bool IsNeedCheckPieces;

        public GridData()
        {
            Rows = 24;
            Columns = 10;
            FillMatrix = new bool[Rows, Columns];
        }
    }
}