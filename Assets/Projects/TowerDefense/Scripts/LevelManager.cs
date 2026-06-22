using System;
using System.Collections.Generic;
using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Drives the level/wave progression: activates the current level's path + slots, shows
    /// banners, tells the spawner to run each wave, detects clears, advances, and triggers the
    /// win when all 3 levels are done. Each level starts fresh (gold, base HP, towers reset).
    /// Replaces OrbitGunner's DifficultyDirector.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        public event Action Changed;

        [SerializeField] private GameObject[] _levelContainers;
        [SerializeField] private EnemyPath[] _paths;
        [SerializeField] private float _levelBannerTime = 2.5f;
        [SerializeField] private float _waveBannerTime = 2.0f;

        public int LevelIndex { get; private set; }
        public int WaveIndex { get; private set; }
        public int LevelCount => _levels != null ? _levels.Length : 3;
        public int WaveCount => 3;
        public string BannerText { get; private set; } = "";

        public EnemyPath ActivePath =>
            _paths != null && LevelIndex >= 0 && LevelIndex < _paths.Length ? _paths[LevelIndex] : null;

        public IReadOnlyList<TowerSlot> ActiveSlots => _activeSlots;

        private LevelConfig[] _levels;
        private readonly List<TowerSlot> _activeSlots = new List<TowerSlot>();

        private enum Phase { Banner, Spawning, WaitClear }
        private Phase _phase;
        private float _timer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _levels = LevelLibrary.Build();
        }

        private void Start()
        {
            StartLevel(0);
        }

        private void StartLevel(int index)
        {
            LevelIndex = Mathf.Clamp(index, 0, LevelCount - 1);
            WaveIndex = 0;

            if (_levelContainers != null)
            {
                for (int i = 0; i < _levelContainers.Length; i++)
                {
                    if (_levelContainers[i] != null)
                        _levelContainers[i].SetActive(i == LevelIndex);
                }
            }

            CollectSlots();

            if (ResourceManager.Instance != null)
                ResourceManager.Instance.SetGold(ResourceManager.Instance.StartingGold);
            if (BaseHealth.Instance != null)
                BaseHealth.Instance.ResetFull();
            if (EnemySpawner.Instance != null)
                EnemySpawner.Instance.StopAndClear();
            if (BuildManager.Instance != null)
                BuildManager.Instance.ClearAllTowers();

            BeginBanner($"Рівень {LevelIndex + 1}", _levelBannerTime);
        }

        private void CollectSlots()
        {
            _activeSlots.Clear();
            if (_levelContainers != null && LevelIndex < _levelContainers.Length && _levelContainers[LevelIndex] != null)
                _activeSlots.AddRange(_levelContainers[LevelIndex].GetComponentsInChildren<TowerSlot>(true));
        }

        private void BeginBanner(string text, float time)
        {
            BannerText = text;
            _phase = Phase.Banner;
            _timer = time;
            Changed?.Invoke();
        }

        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing)
                return;
            if (_levels == null)
                return;

            switch (_phase)
            {
                case Phase.Banner:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        BannerText = "";
                        if (EnemySpawner.Instance != null)
                            EnemySpawner.Instance.BeginWave(_levels[LevelIndex], WaveIndex);
                        _phase = Phase.Spawning;
                        Changed?.Invoke();
                    }
                    break;

                case Phase.Spawning:
                    if (EnemySpawner.Instance == null || EnemySpawner.Instance.SpawnComplete)
                        _phase = Phase.WaitClear;
                    break;

                case Phase.WaitClear:
                    if (EnemyRegistry.Count == 0)
                        OnWaveCleared();
                    break;
            }
        }

        private void OnWaveCleared()
        {
            if (WaveIndex < WaveCount - 1)
            {
                WaveIndex++;
                BeginBanner($"Хвиля {WaveIndex + 1}", _waveBannerTime);
            }
            else if (LevelIndex < LevelCount - 1)
            {
                StartLevel(LevelIndex + 1);
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerWin();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
