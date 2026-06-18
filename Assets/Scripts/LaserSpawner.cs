using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject laserPrefab;

    [Header("Spawn Interval")]
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 8f;

    [Header("Spawn Boundaries")]
    public float minY = -4f;
    public float maxY = 4f;

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
            SpawnLaser();
            timer = 0f;
            SetNextSpawnInterval();
        }
    }

    private void SetNextSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void SpawnLaser()
    {
        float randomY = Random.Range(minY, maxY);
        Instantiate(laserPrefab, new Vector2(0f, randomY), Quaternion.identity);
    }
}