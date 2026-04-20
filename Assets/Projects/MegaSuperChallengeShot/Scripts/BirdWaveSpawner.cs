using UnityEngine;

public class BirdWaveSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Bird normalBirdPrefab;
    [SerializeField] private Bird fastBirdPrefab;
    [SerializeField] private Bird bonusBirdPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float intervalDecreaseRate = 0.02f;

    [Header("Spawn Area")]
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float spawnOffsetX = 0.5f;

    [Header("Probabilities")]
    [SerializeField] [Range(0f, 1f)] private float fastBirdChance = 0.25f;
    [SerializeField] [Range(0f, 1f)] private float bonusBirdChance = 0.1f;

    private Camera _cam;
    private float _timer;
    private float _currentInterval;

    private void Start()
    {
        _cam = Camera.main;
        _currentInterval = spawnInterval;
        _timer = 1f;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameState.Playing) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnBird();
            _currentInterval = Mathf.Max(minSpawnInterval,
                _currentInterval - intervalDecreaseRate);
            _timer = _currentInterval;
        }
    }

    private void SpawnBird()
    {
        // Обираємо тип птаха
        Bird prefab = ChoosePrefab();
        if (prefab == null) return;

        bool fromLeft = Random.value > 0.5f;

        float screenEdgeX;
        Vector2 direction;

        if (fromLeft)
        {
            float leftEdge = _cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            screenEdgeX = leftEdge - spawnOffsetX;
            direction = Vector2.right;
        }
        else
        {
            float rightEdge = _cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
            screenEdgeX = rightEdge + spawnOffsetX;
            direction = Vector2.left;
        }

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(screenEdgeX, randomY, 0f);

        Bird bird = Instantiate(prefab, spawnPos, Quaternion.identity);
        bird.SetDirection(direction);
    }

    private Bird ChoosePrefab()
    {
        float roll = Random.value;

        if (roll < bonusBirdChance && bonusBirdPrefab != null)
            return bonusBirdPrefab;

        if (roll < bonusBirdChance + fastBirdChance && fastBirdPrefab != null)
            return fastBirdPrefab;

        return normalBirdPrefab;
    }
}
