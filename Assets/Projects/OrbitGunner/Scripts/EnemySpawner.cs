using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Sprite _circleSprite;
        [SerializeField] private Sprite _triangleSprite;
        [SerializeField] private Transform _container;
        [SerializeField] private float _spawnMargin = 1.5f;

        private float _timer;
        private float _spawnRadius = 11f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            RecalculateSpawnRadius();
            _timer = 1f;
        }

        public float SpawnRadius => _spawnRadius;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _timer -= Time.deltaTime;
            if (_timer > 0f)
                return;

            SpawnOne();

            float interval = DifficultyDirector.Instance != null
                ? DifficultyDirector.Instance.CurrentSpawnInterval
                : 1f;
            _timer = interval;
        }

        private void SpawnOne()
        {
            if (_enemyPrefab == null)
                return;

            EnemyType type = EnemyConfig.WeightedRandom();
            EnemyConfig config = EnemyConfig.For(type);

            if (DifficultyDirector.Instance != null)
                config.Speed *= DifficultyDirector.Instance.SpeedMultiplier;

            float angle = Random.value * Mathf.PI * 2f;
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * _spawnRadius;

            Sprite sprite = type == EnemyType.Runner ? _triangleSprite : _circleSprite;

            Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _container);
            enemy.Init(config, position, sprite);
        }

        private void RecalculateSpawnRadius()
        {
            Camera cam = Camera.main;
            if (cam != null && cam.orthographic)
            {
                float halfHeight = cam.orthographicSize;
                float halfWidth = halfHeight * cam.aspect;
                _spawnRadius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight) + _spawnMargin;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
