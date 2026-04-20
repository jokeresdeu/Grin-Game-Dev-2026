using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ClassicPlatformer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("UI Ч Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("UI Ч Game Over")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        [Header("UI Ч Level Complete")]
        [SerializeField] private GameObject _levelCompletePanel;
        [SerializeField] private TextMeshProUGUI _levelScoreText;

        private int _score;
        public int Score => _score;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            if (_levelCompletePanel != null)
                _levelCompletePanel.SetActive(false);

            UpdateScoreUI();
        }

        // --- Score ---

        public void AddScore(int amount)
        {
            _score += amount;
            UpdateScoreUI();
        }

        // ƒл€ сум≥сност≥ з≥ старим Pickup.cs
        public void AddCoins(int amount)
        {
            AddScore(amount);
        }

        public void ResetScore()
        {
            _score = 0;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        // --- Game Over ---

        public void OnPlayerDied()
        {
            ShowGameOver();
        }

        private void ShowGameOver()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            if (_finalScoreText != null)
                _finalScoreText.text = $"Score: {_score}";

            Time.timeScale = 0f;
        }

        // ¬икликаЇтьс€ кнопкою "Restart" у GameOver Panel
        public void ShowLevelComplete()
        {
            if (_levelCompletePanel != null)
                _levelCompletePanel.SetActive(true);

            if (_levelScoreText != null)
                _levelScoreText.text = $"Score: {_score}";

            Time.timeScale = 0f;
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // ¬икликаЇтьс€ кнопкою "Main Menu" (€кщо Ї)
        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}