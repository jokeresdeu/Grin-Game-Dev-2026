using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

        [Header("Game Stats")]
        [SerializeField] private int _maxMissed = 5;
        [SerializeField] private int _enemiesToWin = 30;

        private int _score;
        private int _missed;
        private int _killed;

        [Header("UI")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _missedText;

        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private TMP_Text _winScoreText;

        [SerializeField] private GameObject _pausePanel;

        private readonly Dictionary<int, List<Enemy>> _enemiesByLane = new();
        private readonly Dictionary<int, List<Tower>> _towersByLane = new();

        private float _spawnTimer;
        private bool _isGameOver;

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
            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_pausePanel != null) _pausePanel.SetActive(false);

            _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);

            UpdateUI();
        }

        private void Update()
        {
            if (_isGameOver) return;

            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();

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
            int index = Random.Range(0, _spawnPoints.Length);
            var spawnPoint = _spawnPoints[index];

            Enemy enemy = spawnPoint.Spawn();

            if (enemy != null)
                RegisterEnemy(enemy, index);
        }

        public void RegisterEnemy(Enemy enemy, int lane)
        {
            _enemiesByLane[lane].Add(enemy);

            enemy.Initialize();

            enemy.OnDeath += () =>
            {
                AddScore(10);
            };
        }

        public void RegisterTower(Tower tower, int lane)
        {
            _towersByLane[lane].Add(tower);
        }

        private void CleanupLists()
        {
            foreach (var list in _enemiesByLane.Values)
                list.RemoveAll(e => e == null);

            foreach (var list in _towersByLane.Values)
                list.RemoveAll(t => t == null);
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

        public void AddScore(int amount)
        {
            if (_isGameOver) return;

            _score += amount;
            _killed++;

            UpdateUI();

            if (_killed >= _enemiesToWin)
                WinGame();
        }

        public void EnemyMissed()
        {
            if (_isGameOver) return;

            _missed++;
            UpdateUI();

            if (_missed >= _maxMissed)
                GameOver();
        }

        private void WinGame()
        {
            if (_isGameOver) return;

            _isGameOver = true;
            Time.timeScale = 0f;

            if (_winPanel != null)
            {
                _winPanel.SetActive(true);

                if (_winScoreText != null)
                    _winScoreText.text = $"Your final score: {_score}";
            }
        }

        private void GameOver()
        {
            if (_isGameOver) return;

            _isGameOver = true;
            Time.timeScale = 0f;

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);
        }

        private void UpdateUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";

            if (_missedText != null)
                _missedText.text = $"Missed: {_missed}/{_maxMissed}";
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Resume()
        {
            if (_pausePanel != null)
                _pausePanel.SetActive(false);

            Time.timeScale = 1f;
        }

        public void Quit()
        {
            Application.Quit();
        }
        public void LoadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void TogglePause()
        {
            if (_pausePanel == null) return;

            bool isActive = _pausePanel.activeSelf;
            _pausePanel.SetActive(!isActive);

            Time.timeScale = isActive ? 1f : 0f;
        }
    }
}