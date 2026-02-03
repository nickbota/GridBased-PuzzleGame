using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Core;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameController gameController;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI gemsProgressText;
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private void OnEnable()
        {
            if (gameController == null)
            {
                Debug.LogError("GameController reference is not assigned in UIManager.");
                return;
            }

            gameController.OnGemsFoundChanged += UpdateGemsProgress;
            gameController.OnLivesChanged += UpdateLives;
            gameController.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            if (gameController != null)
            {
                gameController.OnGemsFoundChanged -= UpdateGemsProgress;
                gameController.OnLivesChanged -= UpdateLives;
                gameController.OnGameStateChanged -= HandleGameStateChanged;
            }
        }

        private void Start()
        {
            if (winPanel != null)
                winPanel.SetActive(false);

            if (losePanel != null)
                losePanel.SetActive(false);

            InitializeUI();
        }

        private void InitializeUI()
        {
            if (gameController == null)
                return;

            UpdateGemsProgress(gameController.GemsFoundCount, gameController.TotalGemsCount);
            UpdateLives(gameController.CurrentLives);
        }

        private void UpdateGemsProgress(int foundCount, int totalCount)
        {
            if (gemsProgressText != null)
                gemsProgressText.text = $"GEMS FOUND: {foundCount} / {totalCount}";
        }

        private void UpdateLives(int lives)
        {
            if (livesText != null)
                livesText.text = $"{lives}";
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
            if (winPanel != null)
                winPanel.SetActive(true);
        }

        private void ShowLosePanel()
        {
            if (losePanel != null)
                losePanel.SetActive(true);
        }

        private void HidePanels()
        {
            if (winPanel != null)
                winPanel.SetActive(false);

            if (losePanel != null)
                losePanel.SetActive(false);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}