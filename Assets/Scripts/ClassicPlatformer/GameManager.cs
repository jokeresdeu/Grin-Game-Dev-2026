using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ClassicPlatformer
{
    public enum GameState
    {
        Playing,
        GameOver,
        Win
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("UI — Score")]
        [SerializeField] private TextMeshProUGUI _coinsText;

        [Header("UI — Game State Panels")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _winPanel;

        [Header("UI — Game Over")]
        [SerializeField] private TextMeshProUGUI _finalScoreText;

        private int _coins;
        private GameState _currentState = GameState.Playing;

        public int Coins => _coins;
        public GameState CurrentState => _currentState;

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
            UpdateCoinsUI();

            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
            if (_winPanel != null) _winPanel.SetActive(false);

            Time.timeScale = 1f;
        }

        public void AddCoins(int amount)
        {
            if (_currentState != GameState.Playing) return;
            _coins += amount;
            UpdateCoinsUI();
        }

        public void ResetCoins()
        {
            _coins = 0;
            UpdateCoinsUI();
        }

        public void SetGameOver()
        {
            if (_currentState != GameState.Playing) return;
            _currentState = GameState.GameOver;

            Time.timeScale = 0f;

            if (_finalScoreText != null)
                _finalScoreText.text = $"Score: {_coins}";

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);
        }

        public void SetWin()
        {
            if (_currentState != GameState.Playing) return;
            _currentState = GameState.Win;

            Time.timeScale = 0f;

            if (_winPanel != null)
                _winPanel.SetActive(true);
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UpdateCoinsUI()
        {
            if (_coinsText != null)
                _coinsText.text = $"Coins: {_coins}";
        }
    }
}