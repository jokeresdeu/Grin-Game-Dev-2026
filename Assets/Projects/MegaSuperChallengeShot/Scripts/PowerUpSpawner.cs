using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private PowerUp powerUpPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 12f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    private float _timer;

    private void Start()
    {
        _timer = spawnInterval;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameState.Playing) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnPowerUp();
            _timer = spawnInterval;
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUpPrefab == null) return;

        Vector3 pos = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        Instantiate(powerUpPrefab, pos, Quaternion.identity);
    }
}
