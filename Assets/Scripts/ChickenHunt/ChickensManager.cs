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

        [Header("Chest Spawn Points")]
        [SerializeField] private ChestSpawnPoint[] _chestSpawnPoints;

        [Header("Chest Spawn Settings")]
        [SerializeField] private float _minChestSpawnTime = 8f;
        [SerializeField] private float _maxChestSpawnTime = 15f;

        [Header("Bounds")]
        [SerializeField] private float _killDistance = 15f;

        [Header("Game Rules")]
        [SerializeField] private int _winScore = 2000;
        [SerializeField] private int _maxHp = 3;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private BuffPopupUI _buffPopup;

        [Header("Gameplay References")]
        [SerializeField] private Weapon _weapon;

        [Header("Audio")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioClip[] _spawnSounds;

        private readonly List<Chicken> _activeChickens = new();

        private float _spawnTimer;
        private float _chestSpawnTimer;

        private int _score;
        private int _currentHp;

        private bool _isSpawning;
        private bool _gameEnded;
        private bool _chestActive;

        private Chest _activeChest;

        private void Start()
        {
            StartSpawning();
            PlayBackgroundMusic();
            HideEndPanels();

            if (_buffPopup != null)
                _buffPopup.Hide();
        }

        private void Update()
        {
            if (!_isSpawning || _gameEnded) return;

            UpdateSpawning();
            UpdateChestSpawning();
            CheckOutOfBounds();
            CheckChestOutOfBounds();
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

        private void UpdateChestSpawning()
        {
            if (_chestActive) return;
            if (_chestSpawnPoints == null || _chestSpawnPoints.Length == 0) return;

            _chestSpawnTimer -= Time.deltaTime;

            if (_chestSpawnTimer <= 0f)
            {
                SpawnChest();
                _chestSpawnTimer = Random.Range(_minChestSpawnTime, _maxChestSpawnTime);
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

        private void SpawnChest()
        {
            int pointIndex = Random.Range(0, _chestSpawnPoints.Length);
            ChestSpawnPoint spawnPoint = _chestSpawnPoints[pointIndex];

            if (spawnPoint == null)
                return;

            Chest chest = spawnPoint.Spawn(this);

            if (chest != null)
            {
                _activeChest = chest;
                _chestActive = true;
            }
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
                    RegisterChickenEscape(chicken);
                }
            }
        }

        private void CheckChestOutOfBounds()
        {
            if (_activeChest == null)
            {
                _chestActive = false;
                return;
            }

            if (_activeChest.transform.position.magnitude > _killDistance)
            {
                Destroy(_activeChest.gameObject);
                _activeChest = null;
                _chestActive = false;
            }
        }

        public void RegisterChickenEscape(Chicken chicken)
        {
            if (_gameEnded || chicken == null)
                return;

            if (!_activeChickens.Contains(chicken))
                return;

            chicken.OnDeath -= OnChickenDeath;
            _activeChickens.Remove(chicken);

            Destroy(chicken.gameObject);

            LoseHp();
        }

        private void LoseHp()
        {
            _currentHp--;
            UpdateHpUI();

            if (_currentHp <= 0)
            {
                EndGame(false);
            }
        }

        public void OnChestOpened(Chest chest)
        {
            if (_gameEnded) return;

            if (_activeChest == chest)
                _activeChest = null;

            _chestActive = false;

            ApplyRandomBuff();
        }

        private void ApplyRandomBuff()
        {
            int buffId = Random.Range(0, 4);

            string buffTitle;
            string buffDescription;

            switch (buffId)
            {
                case 0:
                    _score += 1000;
                    UpdateScoreUI();

                    buffTitle = "JACKPOT!";
                    buffDescription = "+1000 points to your score.";
                    break;

                case 1:
                    if (_weapon != null)
                        _weapon.AddMaxAmmo(1);

                    buffTitle = "BIGGER MAGAZINE";
                    buffDescription = "+1 bullet to your magazine size. Example: 5 -> 6.";
                    break;

                case 2:
                    if (_weapon != null)
                        _weapon.RefillAmmo();

                    buffTitle = "FULL RELOAD";
                    buffDescription = "Your weapon magazine is instantly refilled to maximum.";
                    break;

                default:
                    if (_currentHp < _maxHp)
                    {
                        _currentHp++;
                        UpdateHpUI();

                        buffTitle = "EXTRA HP";
                        buffDescription = "+1 HP restored.";
                    }
                    else
                    {
                        _score += 500;
                        UpdateScoreUI();

                        buffTitle = "BONUS SCORE";
                        buffDescription = "HP is already full, so you get +500 points instead.";
                    }
                    break;
            }

            PauseForBuff(buffTitle, buffDescription);
        }

        private void PauseForBuff(string title, string description)
        {
            Time.timeScale = 0f;

            if (_buffPopup != null)
                _buffPopup.Show(title, description);
        }

        public void ContinueAfterBuff()
        {
            if (_buffPopup != null)
                _buffPopup.Hide();

            Time.timeScale = 1f;
            CheckWinCondition();
        }

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

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;
            _chestSpawnTimer = Random.Range(_minChestSpawnTime, _maxChestSpawnTime);

            _score = 0;
            _currentHp = _maxHp;
            _gameEnded = false;
            _chestActive = false;
            _activeChest = null;

            UpdateScoreUI();
            UpdateHpUI();
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

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        private void UpdateHpUI()
        {
            if (_hpText != null)
                _hpText.text = $"HP: {_currentHp}";
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

        private void OnDestroy()
        {
            foreach (Chicken chicken in _activeChickens)
            {
                if (chicken != null)
                    chicken.OnDeath -= OnChickenDeath;
            }
        }
    }
}