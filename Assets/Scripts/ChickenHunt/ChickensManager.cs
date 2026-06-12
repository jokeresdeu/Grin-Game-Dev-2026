using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

namespace ChickenHunt
{
    public class ChickensManager : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 1f;
        [SerializeField] private float _maxSpawnTime = 3f;
        [SerializeField] private int _maxChickens = 10;

        [Header("Bounds")]
        [SerializeField] private float _killDistance = 15f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Налаштування Лабораторної")]
        [SerializeField] private Slider _hpSlider;         
        [SerializeField] private GameObject _gameOverPanel; 
        [SerializeField] private int _maxHealth = 100;      
        
        private int _currentHealth;
        private bool _isGameOver = false;

        private readonly List<Chicken> _activeChickens = new();
        private float _spawnTimer;
        private int _score;
        private bool _isSpawning;

        private void Start()
        {
            _currentHealth = _maxHealth;
            if (_hpSlider != null)
            {
                _hpSlider.maxValue = _maxHealth;
                _hpSlider.value = _currentHealth;
            }
            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);

            StartSpawning();
        }

        private void Update()
        {
            if (_isGameOver) return; 
            if (!_isSpawning) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        // Отримання шкоди від курчат
        public void TakeDamage(int damage)
        {
            if (_isGameOver) return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            if (_hpSlider != null) _hpSlider.value = _currentHealth;

            if (_currentHealth <= 0) 
            {
                GameOver();
            }
        }

        // Додавання балів (для Скрині або лову)
        public void AddScore(int points)
        {
            if (_isGameOver) return;
            _score += points;
            UpdateScoreUI();
        }

        // Кінець гри (ВИПРАВЛЕНО ПОРЯДОК УВІМКНЕННЯ UI)
        private void GameOver()
        {
            _isGameOver = true;
            StopSpawning();
            
            // Спочатку робимо панель колірною та видимою
            if (_gameOverPanel != null) 
            {
                _gameOverPanel.SetActive(true);
            }
            
            // Тільки потім повністю зупиняємо ігровий час
            Time.timeScale = 0f; 
        }

        public void RestartGame()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

                if (chicken == null)
                {
                    _activeChickens.RemoveAt(i);
                    continue;
                }

                if (chicken.transform.position.magnitude > _killDistance)
                {
                    chicken.OnDeath -= OnChickenDeath;
                    _activeChickens.RemoveAt(i);
                    Destroy(chicken.gameObject);
                }
            }
        }

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;
            _score = 0;
            UpdateScoreUI();
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        private void SpawnChicken()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0) 
                return;

            int pointIndex = Random.Range(0, _spawnPoints.Length);
            SpawnPoint spawnPoint = _spawnPoints[pointIndex];

            if (spawnPoint == null) 
                return;

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
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        private void OnDestroy()
        {
            foreach (var chicken in _activeChickens)
            {
                if (chicken != null)
                {
                    chicken.OnDeath -= OnChickenDeath;
                }
            }
        }
    }
}