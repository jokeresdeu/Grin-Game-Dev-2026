using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    [System.Serializable]
    public class ChickenSpawnVariant
    {
        public Chicken prefab;
        public float weight = 1f;
    }

    public class ChickensManager : MonoBehaviour
    {
        public static ChickensManager Instance { get; private set; }

        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Chicken Variants")]
        [SerializeField] private ChickenSpawnVariant[] _chickenVariants;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 1f;
        [SerializeField] private float _maxSpawnTime = 3f;
        [SerializeField] private int _maxChickens = 10;

        [Header("Bounds")]
        [SerializeField] private float _killDistance = 15f;

        [Header("UI & Game State")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private int _maxHP = 5;

        private readonly List<Chicken> _activeChickens = new List<Chicken>();

        private float _spawnTimer;
        private int _score;
        private bool _isSpawning;
        private int _currentHP;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _currentHP = _maxHP;
            _score = 0;

            UpdateHPUI();
            UpdateScoreUI();

            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(false);
            }

            Time.timeScale = 1f;
            StartSpawning();
        }

        private void Update()
        {
            if (!_isSpawning) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;
        }

        public void StopSpawning()
        {
            _isSpawning = false;
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

        private void SpawnChicken()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
                return;

            SpawnPoint spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            if (spawnPoint == null)
                return;

            Chicken chickenPrefab = GetRandomChickenPrefab();

            if (chickenPrefab == null)
                return;

            Chicken chicken = spawnPoint.Spawn(chickenPrefab);

            if (chicken != null)
            {
                chicken.OnDeath += OnChickenDeath;
                _activeChickens.Add(chicken);
            }
        }

        private Chicken GetRandomChickenPrefab()
        {
            if (_chickenVariants == null || _chickenVariants.Length == 0)
                return null;

            float totalWeight = 0f;

            foreach (ChickenSpawnVariant variant in _chickenVariants)
            {
                if (variant != null && variant.prefab != null && variant.weight > 0f)
                {
                    totalWeight += variant.weight;
                }
            }

            if (totalWeight <= 0f)
                return null;

            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (ChickenSpawnVariant variant in _chickenVariants)
            {
                if (variant == null || variant.prefab == null || variant.weight <= 0f)
                    continue;

                currentWeight += variant.weight;

                if (randomValue <= currentWeight)
                {
                    return variant.prefab;
                }
            }

            return null;
        }

        private void CheckOutOfBounds()
        {
            for (int i = _activeChickens.Count - 1; i >= 0; i--)
            {
                Chicken chicken = _activeChickens[i];

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

        private void OnChickenDeath(int points)
        {
            _score += points;
            UpdateScoreUI();
        }

        public void TakeDamage(int damage)
        {
            if (!_isSpawning) return;

            _currentHP -= damage;
            UpdateHPUI();

            if (_currentHP <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            StopSpawning();

            if (_gameOverPanel != null)
            {
                _gameOverPanel.SetActive(true);
            }

            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
            {
                _scoreText.text = $"Score: {_score}";
            }
        }

        private void UpdateHPUI()
        {
            if (_hpText != null)
            {
                _hpText.text = $"HP: {_currentHP}";
            }
        }

        private void OnDestroy()
        {
            foreach (Chicken chicken in _activeChickens)
            {
                if (chicken != null)
                {
                    chicken.OnDeath -= OnChickenDeath;
                }
            }
        }
    }
}