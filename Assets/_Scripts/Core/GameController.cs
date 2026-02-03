using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using Visual;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private LevelConfiguration gameConfiguration;

        [Header("References")]
        [SerializeField] private GridView gridView;

        private GridData gridData;
        private GridGenerator gridGenerator;
        private List<GemData> placedGems;

        public int TotalGemsCount => placedGems != null ? placedGems.Count : 0;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (gameConfiguration == null)
            {
                Debug.LogError("GameConfiguration is not assigned in GameController.");
                return;
            }

            gridData = new GridData(gameConfiguration.GridWidth, gameConfiguration.GridHeight);
            gridData.OnCellRevealed += OnCellRevealed;

            gridGenerator = new GridGenerator(gridData);

            placedGems = gridGenerator.PlaceGemsRandomly(gameConfiguration.GemDefinitions);

            if (gridView != null)
            {
                gridView.Initialize(gridData);
                gridView.OnCellClicked += HandleCellClicked;
            }
            else
            {
                Debug.LogError("GridView reference is not assigned in GameController.");
            }

            Debug.Log($"Game initialized with {TotalGemsCount} gems on a {gameConfiguration.GridWidth}x{gameConfiguration.GridHeight} grid.");
        }
        
        private void HandleCellClicked(int x, int y)
        {
            CellData cell = gridData.GetCell(x, y);
            
            if (cell == null)
            {
                Debug.LogWarning($"Invalid cell coordinates: ({x}, {y})");
                return;
            }
            
            if (cell.State == CellData.CellState.Revealed)
            {
                Debug.Log($"Cell at ({x}, {y}) is already revealed. Ignoring click.");
                return;
            }
            
            gridData.TryRevealCell(x, y);
        }

        private void OnDestroy()
        {
            if (gridData != null)
                gridData.OnCellRevealed -= OnCellRevealed;
                
            if (gridView != null)
                gridView.OnCellClicked -= HandleCellClicked;
        }

        private void OnCellRevealed(CellData cell)
        {
            Debug.Log($"Cell revealed at ({cell.X}, {cell.Y})");
        }
    }
}