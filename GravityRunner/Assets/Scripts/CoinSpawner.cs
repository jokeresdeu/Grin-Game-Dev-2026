using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 3f;

    public float minY = -3.5f;
    public float maxY = 3.5f;

    void Start()
    {
        StartCoroutine(SpawnCoinsRoutine());
    }

    IEnumerator SpawnCoinsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        if (GameManager.instance == null || GameObject.FindGameObjectWithTag("Player") == null) return;

        float spawnX = transform.position.x;

        bool validPosition = false;
        int attempts = 0;
        Vector3 spawnPos = Vector3.zero;

        while (!validPosition && attempts < 10)
        {
            float randomY = Random.Range(minY, maxY);
            spawnPos = new Vector3(spawnX, randomY, 0f);

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 1f);

            if (hit == null || !hit.gameObject.name.Contains("Obstacle"))
            {
                validPosition = true;
            }
            attempts++;
        }

        if (validPosition)
        {
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}