using UnityEngine;

public class VerticalSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Interval")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 2f;

    [Header("Spawn Boundaries")]
    public float minX = -8f;
    public float maxX = 8f;
    public float bottomY = -6f;
    public float topY = 6f;

    private float timer;
    private float currentSpawnInterval;

    void Start()
    {
        SetNextSpawnInterval();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
            SetNextSpawnInterval();
        }
    }

    private void SetNextSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnEnemy()
    {
        float randomX = Random.Range(minX, maxX);
        bool spawnBottom = Random.value > 0.5f;
        float startY = spawnBottom ? bottomY : topY;

        GameObject enemy = Instantiate(enemyPrefab, new Vector2(randomX, startY), Quaternion.identity);
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.moveDirection = spawnBottom ? Vector2.up : Vector2.down;
        }
    }
}