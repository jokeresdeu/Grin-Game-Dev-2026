using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class ChickensManager : MonoBehaviour
    {
        private enum BuffType
        {
            ExtraAmmo,
            FastReload,
            Heal,
            DoubleScore,
            ClearScreen
        }

        public static ChickensManager Instance { get; private set; }

        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

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
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private int _maxHP = 5;

        [Header("Messages")]
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _messageDuration = 2.5f;

        [Header("Buffs")]
        [SerializeField] private Weapon _weapon;
        [SerializeField] private int _rewardInterval = 1000;
        [SerializeField] private float _buffDuration = 20f;
        [SerializeField] private float _fastReloadMultiplier = 0.55f;
        [SerializeField] private int _healAmount = 1;

        [Header("Boss Fight")]
        [SerializeField] private BossChicken _bossPrefab;
        [SerializeField] private Transform _bossSpawnPoint;
        [SerializeField] private Transform _bossCenterPoint;
        [SerializeField] private TextMeshProUGUI _bossHpText;
        [SerializeField] private int _bossFightScore = 6000;
        [SerializeField] private int _bossDefeatBonus = 1000;
        [SerializeField] private float _bossIntroTime = 3f;

        private readonly List<Chicken> _activeChickens = new List<Chicken>();

        private float _spawnTimer;
        private int _score;
        private bool _isSpawning;
        private bool _isBossFight;
        private bool _bossFightStarted;
        private int _currentHP;
        private int _nextRewardScore;
        private int _scoreMultiplier = 1;

        private BossChicken _currentBoss;
        private Coroutine _messageCoroutine;
        private Coroutine _scoreMultiplierCoroutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            _currentHP = _maxHP;
            _score = 0;
            _nextRewardScore = _rewardInterval;
            _bossFightStarted = false;
            _isBossFight = false;

            UpdateHPUI();
            UpdateScoreUI();

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            if (_winPanel != null)
                _winPanel.SetActive(false);

            if (_messageText != null)
                _messageText.gameObject.SetActive(false);

            if (_bossHpText != null)
                _bossHpText.gameObject.SetActive(false);

            Time.timeScale = 1f;
            StartSpawning();
        }

        private void Update()
        {
            if (!_isSpawning) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        public void TakeDamage(int damage)
        {
            if (!_isSpawning && !_isBossFight) return;

            _currentHP -= damage;
            UpdateHPUI();

            if (_currentHP <= 0)
            {
                GameOver();
            }
        }

        public void ShowMessage(string message, float duration = -1f)
        {
            if (_messageText == null) return;

            if (_messageCoroutine != null)
                StopCoroutine(_messageCoroutine);

            _messageCoroutine = StartCoroutine(MessageRoutine(message, duration));
        }

        private IEnumerator MessageRoutine(string message, float duration)
        {
            _messageText.text = message;
            _messageText.gameObject.SetActive(true);

            yield return new WaitForSeconds(duration > 0f ? duration : _messageDuration);

            _messageText.gameObject.SetActive(false);
            _messageCoroutine = null;
        }

        private void GameOver()
        {
            StopSpawning();
            _isBossFight = false;

            if (_currentBoss != null)
                Destroy(_currentBoss.gameObject);

            if (_bossHpText != null)
                _bossHpText.gameObject.SetActive(false);

            if (_winPanel != null)
                _winPanel.SetActive(false);

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }

        private void WinGame()
        {
            StopSpawning();
            _isBossFight = false;

            ClearAllChickens(false);

            if (_bossHpText != null)
                _bossHpText.gameObject.SetActive(false);

            if (_messageText != null)
                _messageText.gameObject.SetActive(false);

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            if (_winPanel != null)
                _winPanel.SetActive(true);

            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void UpdateHPUI()
        {
            if (_hpText != null)
                _hpText.text = $"HP: {_currentHP}";
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

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;
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
            AddScore(points * _scoreMultiplier);
        }

        private void AddScore(int amount)
        {
            _score += amount;
            UpdateScoreUI();
            CheckScoreRewards();
        }

        private void CheckScoreRewards()
        {
            while (_score >= _nextRewardScore)
            {
                if (_nextRewardScore == _bossFightScore && !_bossFightStarted)
                {
                    _bossFightStarted = true;
                    _nextRewardScore += _rewardInterval;
                    StartCoroutine(StartBossFightRoutine());
                    break;
                }

                GiveRandomBuff();
                _nextRewardScore += _rewardInterval;
            }
        }

        private void GiveRandomBuff()
        {
            BuffType buff = (BuffType)Random.Range(0, 5);

            switch (buff)
            {
                case BuffType.ExtraAmmo:
                    if (_weapon != null)
                    {
                        _weapon.AddMaxAmmo(1);
                        _weapon.RefillAmmo();
                    }

                    ShowMessage("BUFF: +1 magazine ammo");
                    break;

                case BuffType.FastReload:
                    if (_weapon != null)
                        _weapon.ApplyReloadMultiplier(_fastReloadMultiplier, _buffDuration);

                    ShowMessage("BUFF: fast reload for 20 seconds");
                    break;

                case BuffType.Heal:
                    _currentHP = Mathf.Min(_currentHP + _healAmount, _maxHP);
                    UpdateHPUI();

                    ShowMessage("BUFF: +1 HP");
                    break;

                case BuffType.DoubleScore:
                    if (_scoreMultiplierCoroutine != null)
                        StopCoroutine(_scoreMultiplierCoroutine);

                    _scoreMultiplierCoroutine = StartCoroutine(
                        ScoreMultiplierRoutine(2, _buffDuration)
                    );

                    ShowMessage("BUFF: x2 score for 20 seconds");
                    break;

                case BuffType.ClearScreen:
                    ClearAllChickens(false);
                    ShowMessage("BUFF: screen cleared");
                    break;
            }
        }

        private IEnumerator ScoreMultiplierRoutine(int multiplier, float duration)
        {
            _scoreMultiplier = multiplier;

            yield return new WaitForSeconds(duration);

            _scoreMultiplier = 1;
            _scoreMultiplierCoroutine = null;
        }

        private IEnumerator StartBossFightRoutine()
        {
            _isBossFight = true;

            StopSpawning();
            ClearAllChickens(false);

            ShowMessage(
                "BOSS FIGHT!\nStage 1: shoot the boss. Deal 10 damage.",
                _bossIntroTime
            );

            yield return new WaitForSeconds(_bossIntroTime);

            SpawnBoss();
        }

        private void SpawnBoss()
        {
            Debug.Log("SpawnBoss called");

            if (_bossPrefab == null)
            {
                Debug.LogError("Boss Prefab is not assigned in ChickensManager!");
                ShowMessage("Boss Prefab is not assigned!", 3f);
                _isBossFight = false;
                StartSpawning();
                return;
            }

            Vector3 spawnPosition = _bossSpawnPoint != null
                ? _bossSpawnPoint.position
                : new Vector3(0f, 7f, 0f);

            Vector3 centerPosition = _bossCenterPoint != null
                ? _bossCenterPoint.position
                : Vector3.zero;

            _currentBoss = Instantiate(_bossPrefab, spawnPosition, Quaternion.identity);
            _currentBoss.OnBossDefeated += OnBossDefeated;
            _currentBoss.Initialize(centerPosition, _bossHpText);
        }

        private void OnBossDefeated(BossChicken boss)
        {
            if (boss != null)
                boss.OnBossDefeated -= OnBossDefeated;

            _currentBoss = null;

            AddScore(_bossDefeatBonus);

            WinGame();
        }

        private void ClearAllChickens(bool givePoints)
        {
            Chicken[] allChickens = FindObjectsOfType<Chicken>();

            foreach (Chicken chicken in allChickens)
            {
                if (chicken == null) continue;

                chicken.OnDeath -= OnChickenDeath;

                if (givePoints)
                    chicken.OnShoot();
                else
                    Destroy(chicken.gameObject);
            }

            _activeChickens.Clear();
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        private void OnDestroy()
        {
            foreach (Chicken chicken in _activeChickens)
            {
                if (chicken != null)
                    chicken.OnDeath -= OnChickenDeath;
            }

            if (_currentBoss != null)
                _currentBoss.OnBossDefeated -= OnBossDefeated;
        }
    }
}