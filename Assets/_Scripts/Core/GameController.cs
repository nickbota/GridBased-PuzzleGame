using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GridSystem;
using Visual;
using Zenject;

namespace Core
{
    public class GameController : MonoBehaviour, IGameStatsProvider, ILevelManager
    {
        public event Action<GameState> OnGameStateChanged;
        public event Action<int, int> OnGemsProgressChanged;
        public event Action<int> OnLivesChanged;

        [Header("Configuration")]
        [SerializeField] private LevelConfiguration[] levelConfigurations;

        private GridView _gridView;
        private IGridFactory _gridFactory;
        
        private GridData _gridData;
        private LevelSession _session;
        private List<GemData> _placedGems;
        private LevelConfiguration _selectedLevelConfiguration;
        
        private GameState _currentGameState;

        public int TotalGemsCount => _placedGems != null ? _placedGems.Count : 0;
        public int GemsFoundCount => _session?.GemsFound ?? 0;
        public GameState CurrentGameState => _currentGameState;
        public int CurrentLives => _session?.Lives ?? 0;
        
        int IGameStatsProvider.CurrentGems => GemsFoundCount;
        int IGameStatsProvider.TotalGems => TotalGemsCount;
        int IGameStatsProvider.CurrentLives => CurrentLives;

        [Inject]
        public void Construct(GridView gridView, IGridFactory gridFactory)
        {
            _gridView = gridView;
            _gridFactory = gridFactory;
        }
        private void Start()
        {
            InitializeGame();
        }
        private void OnDestroy()
        {
            if (_gridData != null)
                _gridData.OnCellRevealed -= HandleCellRevealed;

            if (_gridView != null)
                _gridView.OnCellClicked -= HandleCellClicked;

            UnsubscribeFromGemEvents();
        }

        private void InitializeGame()
        {
            //Select level configuration
            if (levelConfigurations == null || levelConfigurations.Length == 0)
            {
                Debug.LogError("No level configurations are assigned in GameController.");
                return;
            }

            _selectedLevelConfiguration = SelectRandomLevelConfiguration();
            
            if (_selectedLevelConfiguration == null)
            {
                Debug.LogError("Selected level configuration is null.");
                return;
            }

            //Create grid, place gems & create session
            _gridData = _gridFactory.CreateGrid(_selectedLevelConfiguration.GridWidth, _selectedLevelConfiguration.GridHeight);
            _placedGems = _gridFactory.PlaceGems(_gridData, _selectedLevelConfiguration.GemDefinitions);
            _session = new LevelSession(_selectedLevelConfiguration.Lives, _placedGems.Count);

            //Initialize gridView and listen to relevant events
            if (_gridView != null)
            {
                _gridView.Initialize(_gridData, _placedGems);
                _gridView.OnCellClicked += HandleCellClicked;
            }
            else
                Debug.LogError("GridView reference is not assigned in GameController.");

            _gridData.OnCellRevealed += HandleCellRevealed;
            SubscribeToGemEvents();

            //Start game and refresh UI
            SetGameState(GameState.Playing);
            RefreshUI();

            Debug.Log($"Game initialized with {TotalGemsCount} gems on a {_selectedLevelConfiguration.GridWidth}x{_selectedLevelConfiguration.GridHeight} grid.");
        }
        
        private LevelConfiguration SelectRandomLevelConfiguration()
        {
            int randomIndex = UnityEngine.Random.Range(0, levelConfigurations.Length);
            LevelConfiguration selected = levelConfigurations[randomIndex];
            Debug.Log($"Selected level configuration {randomIndex + 1} out of {levelConfigurations.Length}");
            return selected;
        }
        
        private void HandleCellClicked(int x, int y)
        {
            if (_currentGameState != GameState.Playing)
            {
                Debug.Log("Game is not in Playing state. Ignoring click.");
                return;
            }
            
            _gridData.TryRevealCell(x, y);
        }

        private void SubscribeToGemEvents()
        {
            if (_placedGems == null)
                return;

            foreach (var gem in _placedGems)
                gem.OnGemFound += HandleGemFound;
        }
        private void UnsubscribeFromGemEvents()
        {
            if (_placedGems == null)
                return;

            foreach (var gem in _placedGems)
                gem.OnGemFound -= HandleGemFound;
        }

        private void HandleGemFound(GemData gem)
        {
            _session.IncrementGems();
            OnGemsProgressChanged?.Invoke(_session.GemsFound, _session.TotalGems);
            
            Debug.Log($"Gem found! Progress: {_session.GemsFound}/{_session.TotalGems}");
            
            if (_session.GemsFound >= _session.TotalGems)
            {
                SetGameState(GameState.Won);
                Debug.Log("All gems found! You won!");
            }
        }

        private void SetGameState(GameState newState)
        {
            if (_currentGameState == newState)
                return;

            _currentGameState = newState;
            OnGameStateChanged?.Invoke(_currentGameState);
            
            Debug.Log($"Game state changed to: {_currentGameState}");
        }

        private void HandleCellRevealed(CellData cell)
        {
            Debug.Log($"Cell revealed at ({cell.X}, {cell.Y})");
            
            if (!cell.HasGem)
            {
                _session.DecrementLife();
                OnLivesChanged?.Invoke(_session.Lives);
                
                Debug.Log($"Life lost! Remaining lives: {_session.Lives}");
                
                if (_session.Lives <= 0)
                {
                    SetGameState(GameState.Lost);
                    Debug.Log("No lives left! You lost!");
                }
            }
        }

        private void RefreshUI()
        {
            OnLivesChanged?.Invoke(_session.Lives);
            OnGemsProgressChanged?.Invoke(_session.GemsFound, _session.TotalGems);
        }
        
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}