using System;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;
using Visual;

namespace Core
{
    // Responsible for managing the overall game state, initializing the grid, handling user interactions, and tracking game progress.
    public class GameController : MonoBehaviour
    {
        public event Action<GameState> OnGameStateChanged;
        public event Action<int, int> OnGemsFoundChanged;
        public event Action<int> OnLivesChanged;

        [Header("Configuration")]
        [SerializeField] private LevelConfiguration levelConfiguration;

        [Header("References")]
        [SerializeField] private GridView gridView;

        private GridData gridData;
        private GridGenerator gridGenerator;
        private List<GemData> placedGems;
        
        private GameState currentGameState;
        private int gemsFoundCount;
        private int currentLives;

        public int TotalGemsCount => placedGems != null ? placedGems.Count : 0;
        public int GemsFoundCount => gemsFoundCount;
        public GameState CurrentGameState => currentGameState;
        public int CurrentLives => currentLives;

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (levelConfiguration == null)
            {
                Debug.LogError("GameConfiguration is not assigned in GameController.");
                return;
            }

            gemsFoundCount = 0;
            currentLives = levelConfiguration.Lives;
            SetGameState(GameState.Playing);
            OnLivesChanged?.Invoke(currentLives);

            gridData = new GridData(levelConfiguration.GridWidth, levelConfiguration.GridHeight);
            gridData.OnCellRevealed += OnCellRevealed;

            gridGenerator = new GridGenerator(gridData);

            placedGems = gridGenerator.PlaceGemsRandomly(levelConfiguration.GemDefinitions);
            
            SubscribeToGemEvents();

            if (gridView != null)
            {
                gridView.Initialize(gridData, placedGems);
                gridView.OnCellClicked += HandleCellClicked;
            }
            else
                Debug.LogError("GridView reference is not assigned in GameController.");

            Debug.Log($"Game initialized with {TotalGemsCount} gems on a {levelConfiguration.GridWidth}x{levelConfiguration.GridHeight} grid.");
        }
        
        private void HandleCellClicked(int x, int y)
        {
            if (currentGameState != GameState.Playing)
            {
                Debug.Log("Game is not in Playing state. Ignoring click.");
                return;
            }
            
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
            
            UnsubscribeFromGemEvents();
        }

        private void SubscribeToGemEvents()
        {
            if (placedGems == null)
                return;

            foreach (var gem in placedGems)
                gem.OnGemFound += OnGemFound;
        }

        private void UnsubscribeFromGemEvents()
        {
            if (placedGems == null)
                return;

            foreach (var gem in placedGems)
                gem.OnGemFound -= OnGemFound;
        }

        private void OnGemFound(GemData gem)
        {
            gemsFoundCount++;
            OnGemsFoundChanged?.Invoke(gemsFoundCount, TotalGemsCount);
            
            Debug.Log($"Gem found! Progress: {gemsFoundCount}/{TotalGemsCount}");
            
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (gemsFoundCount >= TotalGemsCount)
            {
                SetGameState(GameState.Won);
                Debug.Log("All gems found! You won!");
            }
        }

        private void SetGameState(GameState newState)
        {
            if (currentGameState == newState)
                return;

            currentGameState = newState;
            OnGameStateChanged?.Invoke(currentGameState);
            
            Debug.Log($"Game state changed to: {currentGameState}");
        }

        private void OnCellRevealed(CellData cell)
        {
            Debug.Log($"Cell revealed at ({cell.X}, {cell.Y})");
            
            if (!cell.HasGem)
                DecreaseLives();
        }

        private void DecreaseLives()
        {
            if (currentGameState != GameState.Playing)
                return;

            currentLives--;
            OnLivesChanged?.Invoke(currentLives);
            
            Debug.Log($"Life lost! Remaining lives: {currentLives}");
            
            CheckLoseCondition();
        }

        private void CheckLoseCondition()
        {
            if (currentLives <= 0)
            {
                SetGameState(GameState.Lost);
                Debug.Log("No lives left! You lost!");
            }
        }
    }
}