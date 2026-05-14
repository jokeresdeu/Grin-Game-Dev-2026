using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

        [Header("Game Rules")]
        [SerializeField] private int _winScore = 2000;
        [SerializeField] private int _maxEscapedChickens = 5;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        [Header("Audio")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioClip[] _spawnSounds;

        private readonly List<Chicken> _activeChickens = new();

        private float _spawnTimer;
        private int _score;
        private int _escapedChickens;

        private bool _isSpawning;
        private bool _gameEnded;

        // =========================
        // INIT
        // =========================

        private void Start()
        {
            StartSpawning();
            PlayBackgroundMusic();

            HideEndPanels();
        }

        private void Update()
        {
            if (!_isSpawning || _gameEnded) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        // =========================
        // SPAWN LOGIC
        // =========================

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

            int pointIndex = Random.Range(0, _spawnPoints.Length);
            SpawnPoint spawnPoint = _spawnPoints[pointIndex];

            if (spawnPoint == null)
                return;

            int prefabIndex;
            Chicken chicken = spawnPoint.Spawn(out prefabIndex);

            if (chicken != null)
            {
                chicken.OnDeath += OnChickenDeath;
                _activeChickens.Add(chicken);

                PlaySpawnSound(prefabIndex);
            }
        }

        // =========================
        // OUT OF BOUNDS
        // =========================

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

                    RegisterEscape();
                }
            }
        }

        private void RegisterEscape()
        {
            if (_gameEnded) return;

            _escapedChickens++;

            if (_escapedChickens >= _maxEscapedChickens)
            {
                EndGame(false);
            }
        }

        // =========================
        // SCORE / WIN
        // =========================

        private void OnChickenDeath(int points)
        {
            if (_gameEnded) return;

            _score += points;
            UpdateScoreUI();

            _activeChickens.RemoveAll(c => c == null);

            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (_gameEnded) return;

            if (_score >= _winScore)
            {
                EndGame(true);
            }
        }

        // =========================
        // GAME STATE
        // =========================

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;

            _score = 0;
            _escapedChickens = 0;
            _gameEnded = false;

            UpdateScoreUI();
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        private void EndGame(bool win)
        {
            _gameEnded = true;
            _isSpawning = false;

            HideAllPanels();

            if (win)
            {
                if (_winPanel != null)
                    _winPanel.SetActive(true);
            }
            else
            {
                if (_losePanel != null)
                    _losePanel.SetActive(true);
            }
        }

        // =========================
        // RESTART
        // =========================

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // =========================
        // UI
        // =========================

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        private void HideEndPanels()
        {
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_losePanel != null) _losePanel.SetActive(false);
        }

        private void HideAllPanels()
        {
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_losePanel != null) _losePanel.SetActive(false);
        }

        // =========================
        // AUDIO
        // =========================

        private void PlayBackgroundMusic()
        {
            if (_musicSource != null && _backgroundMusic != null)
            {
                _musicSource.clip = _backgroundMusic;
                _musicSource.loop = true;
                _musicSource.Play();
            }
        }

        private void PlaySpawnSound(int index)
        {
            if (_sfxSource == null) return;
            if (_spawnSounds == null || index < 0 || index >= _spawnSounds.Length) return;

            AudioClip clip = _spawnSounds[index];
            if (clip == null) return;

            _sfxSource.pitch = Random.Range(0.95f, 1.05f);
            _sfxSource.PlayOneShot(clip);
        }

        // =========================
        // CLEANUP
        // =========================

        private void OnDestroy()
        {
            foreach (var chicken in _activeChickens)
            {
                if (chicken != null)
                    chicken.OnDeath -= OnChickenDeath;
            }
        }
    }
}