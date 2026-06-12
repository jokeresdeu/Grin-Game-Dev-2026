using UnityEngine;

public class HorizontalSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject enemyPrefab;

    [Header("Spawn Interval")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 2f;

    [Header("Spawn Boundaries")]
    public float minY = -4f;
    public float maxY = 4f;
    public float leftX = -10f;
    public float rightX = 10f;

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
        float randomY = Random.Range(minY, maxY);
        bool spawnLeft = Random.value > 0.5f;
        float startX = spawnLeft ? leftX : rightX;

        GameObject enemy = Instantiate(enemyPrefab, new Vector2(startX, randomY), Quaternion.identity);
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.moveDirection = spawnLeft ? Vector2.right : Vector2.left;
        }
    }
}