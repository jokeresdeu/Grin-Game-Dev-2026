using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClassicPlatformer
{
    public class UIManager : MonoBehaviour
    {
        [Header("HP — Slider")]
        [SerializeField] private Slider _hpSlider;

        [Header("Score — TMP")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("GameOver Panel")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _gameOverScoreText;
        [SerializeField] private Button _restartButton;

        [Header("Player")]
        [SerializeField] private PlayerHealth _playerHealth;

        private void Start()
        {
            // Підписуємося на події
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChanged += UpdateHP;
                _playerHealth.OnDeath += ShowGameOver;
                UpdateHP(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnGameOver += ShowGameOver;
            }

            if (_restartButton != null)
                _restartButton.onClick.AddListener(GameManager.Instance.RestartGame);

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            UpdateScore(GameManager.Score);
        }

        // HP Slider: value 0..1
        private void UpdateHP(int current, int max)
        {
            if (_hpSlider == null) return;
            _hpSlider.maxValue = max;
            _hpSlider.value = current;
        }

        // Score TMP
        private void UpdateScore(int score)
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {score}";
        }

        // GameOver панель
        private void ShowGameOver()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            if (_gameOverScoreText != null)
                _gameOverScoreText.text = $"Score: {GameManager.Score}";
        }

        private void OnDestroy()
        {
            if (_playerHealth != null)
            {
                _playerHealth.OnHealthChanged -= UpdateHP;
                _playerHealth.OnDeath -= ShowGameOver;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnGameOver -= ShowGameOver;
            }
        }
    }
}
