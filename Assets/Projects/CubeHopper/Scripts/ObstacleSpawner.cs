using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class ObstacleEntry
        {
            public GameObject prefab;
            [Range(0.1f, 10f)] public float weight = 1f;
            public float minSpacing = 3f;
        }

        [SerializeField] private ObstacleEntry[] _entries;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _minSpawnInterval = 1.1f;
        [SerializeField] private float _maxSpawnInterval = 2.4f;
        [SerializeField] private float _difficultyCurveSeconds = 45f;
        [SerializeField] private float _minIntervalAtMaxDifficulty = 0.55f;
        [SerializeField] private float _maxIntervalAtMaxDifficulty = 1.1f;
        [SerializeField] private int _maxConcurrentObstacles = 6;

        private float _nextSpawnTime;
        private float _elapsedPlayTime;
        private int _activeCount;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _elapsedPlayTime += Time.deltaTime;

            if (Time.time < _nextSpawnTime)
                return;

            if (_activeCount >= _maxConcurrentObstacles)
            {
                _nextSpawnTime = Time.time + 0.2f;
                return;
            }

            SpawnRandom();
            _nextSpawnTime = Time.time + GetRandomInterval();
        }

        private float GetRandomInterval()
        {
            float t = Mathf.Clamp01(_elapsedPlayTime / _difficultyCurveSeconds);
            float curMin = Mathf.Lerp(_minSpawnInterval, _minIntervalAtMaxDifficulty, t);
            float curMax = Mathf.Lerp(_maxSpawnInterval, _maxIntervalAtMaxDifficulty, t);
            return Random.Range(curMin, curMax);
        }

        private void SpawnRandom()
        {
            if (_entries == null || _entries.Length == 0 || _spawnPoint == null)
                return;

            ObstacleEntry chosen = PickWeighted();
            if (chosen == null || chosen.prefab == null)
                return;

            GameObject instance = Instantiate(chosen.prefab, _spawnPoint.position, Quaternion.identity);
            _activeCount++;
            ObstacleLifecycleNotifier notifier = instance.AddComponent<ObstacleLifecycleNotifier>();
            notifier.Destroyed += OnObstacleDestroyed;
        }

        private ObstacleEntry PickWeighted()
        {
            float total = 0f;
            for (int i = 0; i < _entries.Length; i++)
                total += Mathf.Max(0f, _entries[i].weight);

            if (total <= 0f)
                return _entries[0];

            float roll = Random.value * total;
            float acc = 0f;
            for (int i = 0; i < _entries.Length; i++)
            {
                acc += Mathf.Max(0f, _entries[i].weight);
                if (roll <= acc)
                    return _entries[i];
            }

            return _entries[_entries.Length - 1];
        }

        private void OnObstacleDestroyed()
        {
            _activeCount = Mathf.Max(0, _activeCount - 1);
        }
    }
}
