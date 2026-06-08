using UnityEngine;
using System.Collections.Generic;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadTilePrefab;
    public GameObject crystalPrefab;
    public Transform playerTransform;

    private Vector3 lastPosition;
    private float tileSize = 1.5f;

    private List<GameObject> activeTiles = new List<GameObject>();
    private int startTilesCount = 20;

    [Header("Налаштування спавну монет")]
    public int minStepsBetweenCoins = 3; // Мінімальна кількість порожніх плиток між монетами
    [Range(0f, 100f)]
    public float coinSpawnChance = 40f; // Шанс спавну (у відсотках), якщо мінімальний інтервал пройдено

    private int stepsSinceLastCoin = 0;  // Лічильник плиток, що пройшли з моменту останньої монети

    void Start()
    {
        lastPosition = new Vector3(0, 0f, 0);

        for (int i = 0; i < startTilesCount; i++)
        {
            SpawnTile(i < 6);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // нові платформи на передку
        if (Vector3.Distance(playerTransform.position, lastPosition) < 20f)
        {
            SpawnTile(false);
        }
        CleanupOldTiles();
    }

    void SpawnTile(bool isStart)
    {
        Vector3 spawnPos = lastPosition;

        if (isStart)
        {
            spawnPos.z += tileSize;
        }
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) spawnPos.z += tileSize;
            else spawnPos.x += tileSize;
        }

        GameObject tile = Instantiate(roadTilePrefab, spawnPos, Quaternion.identity);
        activeTiles.Add(tile);
        lastPosition = spawnPos;

        if (!isStart)
        {
            stepsSinceLastCoin++; 

            // Якщо мінімальний інтервал кроків пройдено, пробуємо заспавнити монету
            if (stepsSinceLastCoin >= minStepsBetweenCoins)
            {
                if (Random.Range(0f, 100f) <= coinSpawnChance)
                {
                    Vector3 crystalPos = spawnPos + new Vector3(0, 0.8f, 0);
                    GameObject crystal = Instantiate(crystalPrefab, crystalPos, Quaternion.identity);

                    crystal.transform.parent = tile.transform;

                    stepsSinceLastCoin = 0; // Скидаємо лічильник після успішного спавну
                }
            }
        }
    }

    void CleanupOldTiles()
    {
        // Перевіряємо масив активних плиток безперервно
        while (activeTiles.Count > 0)
        {
            GameObject tileToRemove = activeTiles[0];

            if (tileToRemove == null)
            {
                activeTiles.RemoveAt(0);
                continue;
            }

            // Рахуємо точну відстань між кубом та найстарішою плиткою
            float distance = Vector3.Distance(playerTransform.position, tileToRemove.transform.position);

            //видалення
            if (distance > (tileSize * 2.6f) &&
                (playerTransform.position.x > tileToRemove.transform.position.x ||
                 playerTransform.position.z > tileToRemove.transform.position.z))
            {
                activeTiles.RemoveAt(0);
                StartCoroutine(FallAndDestroy(tileToRemove));
            }
            else
            {
                // Якщо найперша плитка ще близько до гравця — наступні тим паче близько, зупиняємо цикл
                break;
            }
        }
    }

    System.Collections.IEnumerator FallAndDestroy(GameObject tile)
    {
        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startPos = tile.transform.position;
        Vector3 endPos = startPos - new Vector3(0, 8f, 0); // Плитка гарно падає вниз перед знищенням

        while (elapsed < duration)
        {
            if (tile == null) yield break;
            tile.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(tile);
    }
}