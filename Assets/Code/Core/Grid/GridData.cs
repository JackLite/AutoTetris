namespace Core.Grid
{
    public class GridData
    {
        public GridMono Mono;
        public int Rows;
        public int Columns;
        public bool[,] FillMatrix;
        public bool IsGridStable = true;
        public bool IsNeedCheckPieces;

        public GridData(GridMono gridMono)
        {
            Mono = gridMono;
            Rows = 24;
            Columns = 10;
            FillMatrix = new bool[Rows, Columns];
        }
    }
}