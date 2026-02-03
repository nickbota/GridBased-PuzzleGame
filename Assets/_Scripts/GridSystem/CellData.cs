using System;

namespace GridSystem
{
    // Data class representing each cell in the grid and its state
    // GemId represents the gem occupying the cell, if none the ID is -1
    public class CellData
    {
        public enum CellState
        {
            Covered,
            Revealed
        }
        public CellState State { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int GemId { get; private set; }
        public bool HasGem => GemId != NoGemId;
        private const int NoGemId = -1;

        public event Action<CellData> OnStateChanged;

        public CellData(int x, int y)
        {
            X = x;
            Y = y;
            State = CellState.Covered;
            GemId = NoGemId;
        }

        public void SetGem(int gemId)
        {
            GemId = gemId;
        }

        public void Reveal()
        {
            if (State == CellState.Revealed)
                return;

            State = CellState.Revealed;
            OnStateChanged?.Invoke(this);
        }
    }
}