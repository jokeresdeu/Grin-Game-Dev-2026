using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform planeTransform;
    public float spawnRate = 2f;
    public float randomOffset = 1.5f;
    public float clearRadius = 1.5f;

    private float timer;
    private int coinsSpawned = 0;
    private PlaneController plane;

    void Start()
    {
        plane = FindFirstObjectByType<PlaneController>();
    }

    void Update()
    {
        if (plane == null || !plane.gameStarted) return;
        if (coinsSpawned >= 10) return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            float spawnX = transform.position.x + 15f;
            float spawnY = planeTransform.position.y + Random.Range(-randomOffset, randomOffset);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            Collider2D hit = Physics2D.OverlapCircle(spawnPosition, clearRadius);

            if (hit == null)
            {
                // Місце вільне! Створюємо монетку і скидаємо таймер
                GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
                coinsSpawned++;
                Destroy(newCoin, 10f);
                timer = 0f;
            }
            else
            {
                // Якщо там гора, не чекаємо повний цикл (spawnRate)
                // Віднімаємо трохи часу, щоб скрипт спробував ще раз буквально через півсекунди
                timer = spawnRate - 0.5f;
            }
        }
    }
}