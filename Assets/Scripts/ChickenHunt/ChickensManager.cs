using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ChickenHunt
{
    public class ChickensManager : MonoBehaviour
    {
        public static ChickensManager Instance { get; private set; }

        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 1f;
        [SerializeField] private float _maxSpawnTime = 3f;
        [SerializeField] private int _maxChickens = 10;

        [Header("Bounds")]
        [SerializeField] private float _killDistance = 15f;

        [Header("UI System (Lab 3)")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private GameObject _crosshairObject; // Посилання на об'єкт прицілу (Crosshair_01)

        [Header("Health Kit Settings")]
        [SerializeField] private GameObject _healthKitPrefab;

        private readonly List<Chicken> _activeChickens = new();
        private float _spawnTimer;
        private float _healthKitTimer = 10f;
        private int _score;
        private bool _isSpawning;
        private int _currentLives = 5;
        private bool _isGameOver = false; // Запобіжник від спаму при програші

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            Time.timeScale = 1f;
            StartSpawning();
            InitUI();
        }

        private void Update()
        {
            if (!_isSpawning || _isGameOver) return;

            UpdateSpawning();
            UpdateHealthKitSpawn();
            CheckOutOfBounds();
        }

        private void InitUI()
        {
            _currentLives = 5;
            _isGameOver = false; // Скидаємо прапорець при старті

            if (_hpSlider != null)
            {
                _hpSlider.maxValue = 5;
                _hpSlider.value = 5;
            }
            if (_losePanel != null) _losePanel.SetActive(false);
            UpdateScoreUI();
            UpdateHpUI();
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

        private void UpdateHealthKitSpawn()
        {
            _healthKitTimer -= Time.deltaTime;
            if (_healthKitTimer <= 0f)
            {
                _healthKitTimer = 10f;
                if (Random.Range(0f, 100f) <= 18f)
                {
                    SpawnHealthKit();
                }
            }
        }

        private void SpawnHealthKit()
        {
            if (_healthKitPrefab == null || _spawnPoints == null || _spawnPoints.Length == 0) return;

            int index = Random.Range(0, _spawnPoints.Length);
            SpawnPoint point = _spawnPoints[index];

            GameObject kitGo = Instantiate(_healthKitPrefab, point.transform.position, Quaternion.identity);
            if (kitGo.TryGetComponent<HealthKit>(out var healthKit))
            {
                Vector2 dir = point.transform.position.x > 0 ? Vector2.left : Vector2.right;
                healthKit.Initialize(dir);
            }
        }

        private void CheckOutOfBounds()
        {
            if (_isGameOver) return;

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

                    TakeDamage();
                }
            }
        }

        private void TakeDamage()
        {
            if (_isGameOver) return;

            _currentLives--;
            if (_hpSlider != null) _hpSlider.value = _currentLives;
            UpdateHpUI();

            if (_currentLives <= 0)
            {
                TriggerGameOver();
            }
        }

        public void AddLife()
        {
            if (_currentLives < 5 && !_isGameOver)
            {
                _currentLives++;
                if (_hpSlider != null) _hpSlider.value = _currentLives;
                UpdateHpUI();
            }
        }

        private void TriggerGameOver()
        {
            if (_isGameOver) return;
            _isGameOver = true;
            _isSpawning = false;

            Time.timeScale = 0f; // Зупиняємо фізику та час гри

            if (_losePanel != null)
            {
                _losePanel.SetActive(true);
            }

            // ВИМИКАЄМО ПРИЦІЛ ПРИ ПОРАЗЦІ, щоб повернути контроль над мишкою
            if (_crosshairObject != null)
            {
                _crosshairObject.SetActive(false);
            }

            // Звільняємо курсор для кліків по кнопках поразки
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            if (_isGameOver) return;

            _score += points;
            UpdateScoreUI();
            _activeChickens.RemoveAll(c => c == null || !c.gameObject.activeInHierarchy);
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null) _scoreText.text = $"Score: {_score}";
        }

        private void UpdateHpUI()
        {
            if (_hpText != null) _hpText.text = $"HP: {_currentLives}/5";
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