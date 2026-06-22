using System.Collections.Generic;
using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Spawns a wave's enemies onto the active path at a fixed interval, applying the
    /// current level's stat multipliers to a fresh config copy per enemy. Driven by
    /// <see cref="LevelManager"/>; exposes <see cref="SpawnComplete"/> so it knows when
    /// the wave has finished emitting.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _container;

        private readonly Queue<EnemyType> _queue = new Queue<EnemyType>();
        private LevelConfig _level;
        private float _interval;
        private float _timer;
        private bool _spawning;

        public bool SpawnComplete => !_spawning && _queue.Count == 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void BeginWave(LevelConfig level, int waveIndex)
        {
            _level = level;
            WaveConfig wave = level.Waves[waveIndex];

            _queue.Clear();
            foreach (EnemyType type in wave.BuildSpawnOrder())
                _queue.Enqueue(type);

            _interval = wave.SpawnInterval;
            _timer = 0f;
            _spawning = _queue.Count > 0;
        }

        public void StopAndClear()
        {
            _spawning = false;
            _queue.Clear();
        }

        private void Update()
        {
            if (!_spawning)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _timer -= Time.deltaTime;
            if (_timer > 0f)
                return;

            if (_queue.Count == 0)
            {
                _spawning = false;
                return;
            }

            SpawnEnemy(_queue.Dequeue());
            _timer = _interval;
        }

        private void SpawnEnemy(EnemyType type)
        {
            if (_enemyPrefab == null || LevelManager.Instance == null)
                return;

            EnemyPath path = LevelManager.Instance.ActivePath;
            if (path == null)
                return;

            EnemyConfig config = ScaleConfig(EnemyConfig.For(type), _level);
            Enemy enemy = Instantiate(_enemyPrefab, path.GetPoint(0), Quaternion.identity, _container);
            enemy.Init(config, path);
        }

        private static EnemyConfig ScaleConfig(EnemyConfig config, LevelConfig level)
        {
            if (level != null)
            {
                config.MaxHp = Mathf.Max(1, Mathf.RoundToInt(config.MaxHp * level.HpMult));
                config.Bounty = Mathf.Max(1, Mathf.RoundToInt(config.Bounty * level.BountyMult));
                config.Speed *= level.SpeedMult;
            }
            return config;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
