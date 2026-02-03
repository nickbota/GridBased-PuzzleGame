using System;
using System.Linq;
using System.Collections.Generic;

namespace GridSystem
{
    // Represents an instance of a gem placed on the grid
    // It tracks the occupied grid cells and notifies when the gem is fully revealed
    public class GemData
    {
        public event Action<GemData> OnGemFound;

        public int Id { get; private set; }
        public GemDefinition Definition { get; private set; }

        private readonly List<CellData> occupiedCells;
        public IReadOnlyList<CellData> OccupiedCells => occupiedCells;

        public GemData(int id, GemDefinition definition)
        {
            Id = id;
            Definition = definition;
            occupiedCells = new List<CellData>();
        }

        public void AddCell(CellData cell)
        {
            if (!occupiedCells.Contains(cell))
            {
                occupiedCells.Add(cell);
                cell.OnStateChanged += OnCellStateChanged;
            }
        }

        public bool CheckCompletion()
        {
            return occupiedCells.All(cell => cell.State == CellData.CellState.Revealed);
        }

        private void OnCellStateChanged(CellData cell)
        {
            if (CheckCompletion())
                OnGemFound?.Invoke(this);
        }

        public void Dispose()
        {
            foreach (var cell in occupiedCells)
                cell.OnStateChanged -= OnCellStateChanged;

            occupiedCells.Clear();
        }
    }
}