using System;
using UnityEngine;
using Zenject;
using TMPro;
using Core;

namespace UI
{
    // Responsible for managing the game's UI elements, including HUD and end-game screens.
    public class UIManager : MonoBehaviour, IInitializable, IDisposable
    {
        [Header("HUD Elements")]
        [SerializeField] private TextMeshProUGUI _gemsProgressText;
        [SerializeField] private TextMeshProUGUI _livesText;

        [Header("Screens")]
        [SerializeField] private CanvasGroup _winScreen; 
        [SerializeField] private CanvasGroup _loseScreen;

        private IGameStatsProvider _gameStats;
        private ILevelManager _levelManager;

        [Inject]
        public void Construct(IGameStatsProvider gameStats, ILevelManager levelManager)
        {
            _gameStats = gameStats;
            _levelManager = levelManager;
        }
        public void Initialize()
        {
            BindEvents();
            ResetUI();
            
            UpdateGemsProgress(_gameStats.CurrentGems, _gameStats.TotalGems);
            UpdateLives(_gameStats.CurrentLives);
        }
        public void Dispose()
        {
            UnbindEvents();
        }
        private void BindEvents()
        {
            _gameStats.OnGemsProgressChanged += UpdateGemsProgress;
            _gameStats.OnLivesChanged += UpdateLives;
            _gameStats.OnGameStateChanged += HandleGameStateChanged;
        }
        private void UnbindEvents()
        {
            if (_gameStats != null)
            {
                _gameStats.OnGemsProgressChanged -= UpdateGemsProgress;
                _gameStats.OnLivesChanged -= UpdateLives;
                _gameStats.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        public void RestartLevel()
        {
            if(_levelManager != null)
                _levelManager.RestartLevel();
        }

        private void ResetUI()
        {
            ToggleScreen(_winScreen, false);
            ToggleScreen(_loseScreen, false);
        }

        private void UpdateGemsProgress(int found, int total)
        {
            if (_gemsProgressText != null)
                _gemsProgressText.SetText("GEMS FOUND: {0} / {1}", found, total);
        }

        private void UpdateLives(int lives)
        {
            if (_livesText != null)
                _livesText.SetText("{0}", lives);
        }

        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Won:
                    ToggleScreen(_winScreen, true);
                    break;
                case GameState.Lost:
                    ToggleScreen(_loseScreen, true);
                    break;
                case GameState.Playing:
                    ResetUI();
                    break;
            }
        }

        private void ToggleScreen(CanvasGroup group, bool visible)
        {
            if (group == null) return;

            group.alpha = visible ? 1f : 0f;
            group.interactable = visible;
            group.blocksRaycasts = visible;
        }
    }
}