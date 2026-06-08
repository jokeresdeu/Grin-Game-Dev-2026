using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UkraineVsZombies
{
    public class GameManager : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 2f;
        [SerializeField] private float _maxSpawnTime = 4f;
        [SerializeField] private int _maxEnemies = 20;

        [Header("Lanes")]
        [SerializeField] private int _laneCount = 5;

        [Header("UI")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _baseHealthText;
        [SerializeField] private Slider _baseHealthSlider;

        [Header("Base")]
        [SerializeField] private int _baseMaxHealth = 5;

        [Header("UI Animation")]
        [SerializeField] private float _uiPunchScale = 1.2f;
        [SerializeField] private float _uiPunchTime = 0.12f;
        [SerializeField] private float _gameOverPopTime = 0.25f;

        private readonly Dictionary<int, List<Enemy>> _enemiesByLane = new();
        private readonly Dictionary<int, List<Tower>> _towersByLane = new();
        private float _spawnTimer;
        private bool _isGameOver;
        private int _score;
        private int _baseHealth;
        private Coroutine _scorePunchRoutine;
        private Coroutine _basePunchRoutine;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            for (int i = 0; i < _laneCount; i++)
            {
                _enemiesByLane[i] = new List<Enemy>();
                _towersByLane[i] = new List<Tower>();
            }
        }

        private void Start()
        {
            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            _baseHealth = _baseMaxHealth;
            _score = 0;
            UpdateUI();

            _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }

        private void Update()
        {
            if (_isGameOver) return;

            UpdateSpawning();
            CleanupLists();
            UpdateTargets();
        }

        private void UpdateSpawning()
        {
            int totalEnemies = 0;
            foreach (var list in _enemiesByLane.Values)
                totalEnemies += list.Count;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && totalEnemies < _maxEnemies)
            {
                SpawnEnemy();
                _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
            }
        }

        private void SpawnEnemy()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0) return;

            int index = Random.Range(0, _spawnPoints.Length);
            var spawnPoint = _spawnPoints[index];

            if (spawnPoint == null) return;

            Enemy enemy = spawnPoint.Spawn();
            if (enemy != null)
                RegisterEnemy(enemy, index);
        }

        public void RegisterEnemy(Enemy enemy, int lane)
        {
            if (lane < 0 || lane >= _laneCount) return;
            _enemiesByLane[lane].Add(enemy);
        }

        public void RegisterTower(Tower tower, int lane)
        {
            if (lane < 0 || lane >= _laneCount) return;
            _towersByLane[lane].Add(tower);
        }

        private void CleanupLists()
        {
            foreach (var list in _enemiesByLane.Values)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i] == null)
                        list.RemoveAt(i);
                }
            }

            foreach (var list in _towersByLane.Values)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i] == null)
                        list.RemoveAt(i);
                }
            }
        }

        private void UpdateTargets()
        {
            for (int lane = 0; lane < _laneCount; lane++)
            {
                var towers = _towersByLane[lane];
                var enemies = _enemiesByLane[lane];

                foreach (var tower in towers)
                {
                    if (tower == null || !tower.IsAlive) continue;

                    Enemy bestTarget = null;
                    float closestDist = float.MaxValue;

                    foreach (var enemy in enemies)
                    {
                        if (enemy == null || !enemy.IsAlive) continue;

                        float dist = enemy.transform.position.x - tower.transform.position.x;
                        if (dist > 0 && dist <= tower.Range && dist < closestDist)
                        {
                            closestDist = dist;
                            bestTarget = enemy;
                        }
                    }

                    tower.SetTarget(bestTarget);
                }
            }
        }

        public void GameOver()
        {
            if (_isGameOver) return;
            _isGameOver = true;

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            if (_gameOverPanel != null)
                StartCoroutine(PlayGameOverAnimation());
        }

        public void AddScore(int amount)
        {
            if (_isGameOver) return;

            _score += amount;
            UpdateUI();
            PlayTextPunch(_scoreText, ref _scorePunchRoutine, Color.yellow);
        }

        public void DamageBase(int damage)
        {
            if (_isGameOver) return;

            _baseHealth -= damage;
            _baseHealth = Mathf.Max(0, _baseHealth);
            UpdateUI();
            PlayTextPunch(_baseHealthText, ref _basePunchRoutine, Color.red);

            if (_baseHealth <= 0)
            {
                GameOver();
            }
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UpdateUI()
        {
            if (_scoreText != null)
                _scoreText.text = "Score: " + _score;

            if (_baseHealthText != null)
                _baseHealthText.text = "Base HP: " + _baseHealth + "/" + _baseMaxHealth;

            if (_baseHealthSlider != null)
                _baseHealthSlider.value = _baseMaxHealth > 0 ? (float)_baseHealth / _baseMaxHealth : 0f;
        }

        private void PlayTextPunch(TextMeshProUGUI text, ref Coroutine routine, Color flashColor)
        {
            if (text == null) return;

            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(PlayTextPunchAnimation(text, flashColor));
        }

        private IEnumerator PlayTextPunchAnimation(TextMeshProUGUI text, Color flashColor)
        {
            Transform textTransform = text.transform;
            Vector3 startScale = Vector3.one;
            Color startColor = text.color;
            float timer = 0f;

            while (timer < _uiPunchTime)
            {
                timer += Time.deltaTime;
                float t = timer / _uiPunchTime;
                float scale = Mathf.Lerp(_uiPunchScale, 1f, t);
                textTransform.localScale = startScale * scale;
                text.color = Color.Lerp(flashColor, startColor, t);
                yield return null;
            }

            textTransform.localScale = startScale;
            text.color = startColor;
        }

        private IEnumerator PlayGameOverAnimation()
        {
            Transform panelTransform = _gameOverPanel.transform;
            Vector3 targetScale = Vector3.one;
            panelTransform.localScale = Vector3.zero;

            float timer = 0f;
            while (timer < _gameOverPopTime)
            {
                timer += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, timer / _gameOverPopTime);
                panelTransform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                yield return null;
            }

            panelTransform.localScale = targetScale;
        }
    }
}
