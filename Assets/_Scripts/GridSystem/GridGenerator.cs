using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    // Responsible for taking in grid data and placing gems randomly
    public class GridGenerator
    {
        private readonly GridData _gridData;
        private readonly System.Random _random;
        private int _nextGemId;

        public GridGenerator(GridData gridData, int? seed = null)
        {
            _gridData = gridData;
            _random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            _nextGemId = 0;
        }

        public List<GemData> PlaceGemsRandomly(List<GemDefinition> gemDefinitions, int maxAttempts = 100)
        {
            List<GemData> placedGems = new List<GemData>();
            bool[,] occupiedCells = new bool[_gridData.Width, _gridData.Height];

            foreach (GemDefinition gemDef in gemDefinitions)
            {
                bool placed = false;
                int attempts = 0;

                while (!placed && attempts < maxAttempts)
                {
                    int randomX = _random.Next(0, _gridData.Width);
                    int randomY = _random.Next(0, _gridData.Height);

                    if (CanPlaceGem(randomX, randomY, gemDef, occupiedCells))
                    {
                        GemData gem = PlaceGem(randomX, randomY, gemDef, occupiedCells);
                        placedGems.Add(gem);
                        placed = true;
                    }

                    attempts++;
                }

                if (!placed)
                    Debug.LogWarning($"Failed to place gem '{gemDef.GemName}' after {maxAttempts} attempts.");
            }

            return placedGems;
        }

        private bool CanPlaceGem(int x, int y, GemDefinition gemDef, bool[,] occupiedCells)
        {
            if (!IsWithinBounds(x, y, gemDef))
                return false;

            if (HasCollision(x, y, gemDef, occupiedCells))
                return false;

            return true;
        }

        private bool IsWithinBounds(int x, int y, GemDefinition gemDef)
        {
            int endX = x + gemDef.Width;
            int endY = y + gemDef.Height;

            return endX <= _gridData.Width && endY <= _gridData.Height;
        }

        private bool HasCollision(int x, int y, GemDefinition gemDef, bool[,] occupiedCells)
        {
            for (int offsetX = 0; offsetX < gemDef.Width; offsetX++)
            {
                for (int offsetY = 0; offsetY < gemDef.Height; offsetY++)
                {
                    int cellX = x + offsetX;
                    int cellY = y + offsetY;

                    if (occupiedCells[cellX, cellY])
                        return true;
                }
            }

            return false;
        }

        private GemData PlaceGem(int x, int y, GemDefinition gemDef, bool[,] occupiedCells)
        {
            int gemId = _nextGemId++;
            GemData gem = new GemData(gemId, gemDef);

            for (int offsetX = 0; offsetX < gemDef.Width; offsetX++)
            {
                for (int offsetY = 0; offsetY < gemDef.Height; offsetY++)
                {
                    int cellX = x + offsetX;
                    int cellY = y + offsetY;

                    CellData cell = _gridData.GetCell(cellX, cellY);
                    cell.SetGem(gemId);
                    gem.AddCell(cell);
                    occupiedCells[cellX, cellY] = true;
                }
            }

            return gem;
        }
    }
}