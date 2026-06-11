using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Fruit Prefabs")]
    [SerializeField] private GameObject[] fruitPrefabs;

    [Header("Spawn")]
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnOffset = 1.5f;

    [Header("Movement")]
    [SerializeField] private float minForce = 6f;
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float sideUpForce = 3f;

    private Camera mainCamera;
    private float timer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFruit();
        }
    }

    private void SpawnFruit()
    {
        if (fruitPrefabs == null || fruitPrefabs.Length == 0)
            return;

        int index = Random.Range(0, fruitPrefabs.Length);
        GameObject prefab = fruitPrefabs[index];

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject fruit = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = GetDirectionToScreen(spawnPosition);
            float force = Random.Range(minForce, maxForce);

            rb.linearVelocity = direction * force;
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        int side = Random.Range(0, 3);

        if (side == 0)
        {
            return new Vector2(
                Random.Range(min.x, max.x),
                min.y - spawnOffset
            );
        }

        if (side == 1)
        {
            return new Vector2(
                min.x - spawnOffset,
                Random.Range(min.y, max.y)
            );
        }

        return new Vector2(
            max.x + spawnOffset,
            Random.Range(min.y, max.y)
        );
    }

    private Vector2 GetDirectionToScreen(Vector2 spawnPosition)
    {
        Vector2 center = mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        Vector2 direction = (center - spawnPosition).normalized;

        direction.y += Random.Range(0.1f, sideUpForce);
        return direction.normalized;
    }
}