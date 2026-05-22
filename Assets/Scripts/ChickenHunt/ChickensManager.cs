using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class ChickensManager : MonoBehaviour
    {
        public static ChickensManager Instance { get; private set; }

        private static bool _skipMainMenu = false;

        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 1f;
        [SerializeField] private float _maxSpawnTime = 3f;
        [SerializeField] private int _maxChickens = 10;

        [Header("Bounds")]
        [SerializeField] private float _killDistance = 15f;

        [Header("UI Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _pauseMenuPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _gameplayHolder;

        [Header("UI Texts")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private int _maxHP = 5;

        [Header("Animations")]
        [SerializeField] private Animator _heartAnimator;
        [SerializeField] private Animator _gameOverAnimator;

        private readonly List<Chicken> _activeChickens = new();
        private float _spawnTimer;
        private int _score;
        private bool _isSpawning;
        private int _currentHP;
        private bool _isPaused = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _currentHP = _maxHP;
            UpdateHPUI();

            if (_skipMainMenu)
            {
                _skipMainMenu = false;

                //âìèêàºìî ãðó
                if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
                if (_gameplayHolder != null) _gameplayHolder.SetActive(true);
                if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
                if (_gameOverPanel != null) _gameOverPanel.SetActive(false);

                Time.timeScale = 1f;
                _isSpawning = true;
                _score = 0;
                UpdateScoreUI();
            }
            else
            {
                //ïîêàçóº ìåíþ
                Time.timeScale = 0f;
                _isSpawning = false;

                if (_mainMenuPanel != null) _mainMenuPanel.SetActive(true);
                if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
                if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
                if (_gameplayHolder != null) _gameplayHolder.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _isSpawning && !_gameOverPanel.activeSelf)
            {
                if (_isPaused) ResumeGame();
                else PauseGame();
            }

            if (!_isSpawning) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        //ÊÅÐÓÂÀÍÍß ÃÐÎÞ

        public void StartGame()
        {
            if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
            if (_gameplayHolder != null) _gameplayHolder.SetActive(true);

            Time.timeScale = 1f;
            _isSpawning = true;
            _score = 0;
            UpdateScoreUI();
        }

        public void PauseGame()
        {
            _isPaused = true;
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            _isPaused = false;
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        public void RestartGame()
        {
            _skipMainMenu = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void BackToMainMenu()
        {
            _skipMainMenu = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Ãðà çàêðèòà");
        }

        //ËÎÃ²ÊÀ ÃÐÈ

        public void TakeDamage(int damage)
        {
            if (!_isSpawning) return;

            _currentHP -= damage;
            UpdateHPUI();

            if (_heartAnimator != null) _heartAnimator.SetTrigger("Hurt");

            if (_currentHP <= 0) GameOver();
        }

        private void GameOver()
        {
            StopSpawning();

            if (_finalScoreText != null) _finalScoreText.text = $"YOUR SCORE: {_score}";

            if (_gameplayHolder != null) _gameplayHolder.SetActive(false);
            if (_gameOverPanel != null) _gameOverPanel.SetActive(true);

            if (_gameOverAnimator != null) _gameOverAnimator.SetTrigger("Show");

            Time.timeScale = 0f;
        }

        private void UpdateHPUI()
        {
            if (_hpText != null) _hpText.text = $" {_currentHP}";
        }

        private void UpdateSpawning()
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && _activeChickens.Count < _maxChickens)
            {
                SpawnChicken();
                _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
            }
        }

        private void CheckOutOfBounds()
        {
            for (int i = _activeChickens.Count - 1; i >= 0; i--)
            {
                var chicken = _activeChickens[i];
                if (chicken == null) { _activeChickens.RemoveAt(i); continue; }

                if (chicken.transform.position.magnitude > _killDistance)
                {
                    chicken.OnDeath -= OnChickenDeath;
                    _activeChickens.RemoveAt(i);
                    Destroy(chicken.gameObject);
                }
            }
        }

        public void StopSpawning() => _isSpawning = false;

        private void SpawnChicken()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0) return;

            int pointIndex = Random.Range(0, _spawnPoints.Length);
            SpawnPoint spawnPoint = _spawnPoints[pointIndex];
            if (spawnPoint == null) return;

            Chicken chicken = spawnPoint.Spawn();
            if (chicken != null)
            {
                chicken.OnDeath += OnChickenDeath;
                _activeChickens.Add(chicken);
            }
        }

        private void OnChickenDeath(int points)
        {
            _score += points;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null) _scoreText.text = $"Score: {_score}";
        }

        private void OnDestroy()
        {
            foreach (var chicken in _activeChickens)
            {
                if (chicken != null) chicken.OnDeath -= OnChickenDeath;
            }
        }
    }
}