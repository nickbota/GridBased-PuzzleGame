using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using TMPro;
using Core;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _gemsProgressText;
        [SerializeField] private TextMeshProUGUI _livesText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        private GameController _gameController;
        [Inject]
        private void Init(GameController gameController)
        {
            _gameController = gameController;
        }

        private void OnEnable()
        {
            if (_gameController == null)
            {
                Debug.LogError("GameController reference is not assigned in UIManager.");
                return;
            }

            _gameController.OnGemsProgressChanged += UpdateGemsProgress;
            _gameController.OnLivesChanged += UpdateLives;
            _gameController.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            if (_gameController != null)
            {
                _gameController.OnGemsProgressChanged -= UpdateGemsProgress;
                _gameController.OnLivesChanged -= UpdateLives;
                _gameController.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void Start()
        {
            if (_winPanel != null)
                _winPanel.SetActive(false);

            if (_losePanel != null)
                _losePanel.SetActive(false);

            InitializeUI();
        }

        private void InitializeUI()
        {
            if (_gameController == null)
                return;

            UpdateGemsProgress(_gameController.GemsFoundCount, _gameController.TotalGemsCount);
            UpdateLives(_gameController.CurrentLives);
        }

        private void UpdateGemsProgress(int foundCount, int totalCount)
        {
            if (_gemsProgressText != null)
                _gemsProgressText.text = $"GEMS FOUND: {foundCount} / {totalCount}";
        }

        private void UpdateLives(int lives)
        {
            if (_livesText != null)
                _livesText.text = $"{lives}";
        }

        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Won:
                    ShowWinPanel();
                    break;
                case GameState.Lost:
                    ShowLosePanel();
                    break;
                case GameState.Playing:
                    HidePanels();
                    break;
            }
        }

        private void ShowWinPanel()
        {
            if (_winPanel != null)
                _winPanel.SetActive(true);
        }

        private void ShowLosePanel()
        {
            if (_losePanel != null)
                _losePanel.SetActive(true);
        }

        private void HidePanels()
        {
            if (_winPanel != null)
                _winPanel.SetActive(false);

            if (_losePanel != null)
                _losePanel.SetActive(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}