using System;

namespace GridSystem
{
    // Class representing the entire grid structure
    public class GridData
    {
        public event Action<CellData> OnCellRevealed;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly CellData[,] cells;

        public GridData(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException("Width must be greater than 0", nameof(width));

            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0", nameof(height));

            Width = width;
            Height = height;
            cells = new CellData[width, height];

            InitializeCells();
        }
        private void InitializeCells()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                    cells[x, y] = new CellData(x, y);
            }
        }

        public CellData GetCell(int x, int y)
        {
            if (!IsValidCoordinate(x, y))
                throw new ArgumentOutOfRangeException($"Cell coordinates ({x}, {y}) are out of bounds for grid size ({Width}, {Height})");

            return cells[x, y];
        }

        public bool TryRevealCell(int x, int y)
        {
            if (!IsValidCoordinate(x, y))
                return false;

            var cell = cells[x, y];

            if (cell.State == CellData.CellState.Revealed)
                return false;

            cell.Reveal();
            OnCellRevealed?.Invoke(cell);
            return true;
        }

        private bool IsValidCoordinate(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}