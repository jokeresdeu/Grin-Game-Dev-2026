using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class RPGGameManager : MonoBehaviour
    {
        public static RPGGameManager Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private GameObject _gameOverPanel;

        private int _score;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _score = 0;
            UpdateScoreUI();
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(false);
                var btnTrans = _gameOverPanel.transform.Find("RestartButton");
                if (btnTrans != null)
                {
                    var btn = btnTrans.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(RestartGame);
                    }
                }
            }
            Time.timeScale = 1f;
        }

        public void AddScore(int amount)
        {
            _score += amount;
            UpdateScoreUI();
        }

        public void UpdateHealthUI(float currentHealth, float maxHealth)
        {
            if (_healthSlider != null)
            {
                if (maxHealth <= 0) maxHealth = 1f;
                _healthSlider.value = currentHealth / maxHealth;
            }
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
            {
                _scoreText.text = $"Score: {_score}";
            }
        }

        public bool IsGameOver { get; private set; }
        public bool IsArenaPaused { get; private set; }

        private System.Collections.Generic.Dictionary<Assets.DynamicBackgrounds.Scripts.RepeatingBackground, float> _bgOriginalSpeeds = new();

        public void StartVictorySequence()
        {
            IsGameOver = true;
        }

        public void GameOver()
        {
            if (IsGameOver) return;
            IsGameOver = true;
            Debug.Log("GAME OVER TRIGGERED!");
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
                var textObj = _gameOverPanel.transform.Find("GameOverText");
                if (textObj != null)
                {
                    var tmpro = textObj.GetComponent<TextMeshProUGUI>();
                    if (tmpro != null) tmpro.text = "GAME OVER";
                }
            }
            Time.timeScale = 0.0001f;
        }

        public void GameWon()
        {
            IsGameOver = true;
            Debug.Log("GAME WON TRIGGERED!");
            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
                var textObj = _gameOverPanel.transform.Find("GameOverText");
                if (textObj != null)
                {
                    var tmpro = textObj.GetComponent<TextMeshProUGUI>();
                    if (tmpro != null)
                    {
                        tmpro.text = "VICTORY!";
                        tmpro.color = Color.yellow;
                    }
                }
            }
            Time.timeScale = 0.0001f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PauseArena()
        {
            if (IsArenaPaused) return;
            IsArenaPaused = true;
            
            _bgOriginalSpeeds.Clear();
            var bgs = FindObjectsByType<Assets.DynamicBackgrounds.Scripts.RepeatingBackground>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var bg in bgs)
            {
                _bgOriginalSpeeds[bg] = bg.Speed;
                bg.Speed = 0f;
            }
        }

        public void ResumeArena()
        {
            if (!IsArenaPaused) return;
            IsArenaPaused = false;
            
            foreach (var kvp in _bgOriginalSpeeds)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.Speed = kvp.Value;
                }
            }
            _bgOriginalSpeeds.Clear();
        }
    }
}

