using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnRate = 2f;

    [Range(0f, 0.5f)]
    public float distanceDampening = 0.2f;

    private float timer = 0f;

    void Update()
    {
        float speedMultiplier = GameManager.instance.globalSpeedMultiplier;

        float adjustedSpawnMultiplier = speedMultiplier - (speedMultiplier - 1f) * distanceDampening;

        timer += Time.deltaTime * adjustedSpawnMultiplier;

        if (timer >= spawnRate)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject selectedPrefab = obstaclePrefabs[randomIndex];

            float spawnY = 0f;

            if (selectedPrefab.name.ToLower().Contains("laser"))
            {
                spawnY = Random.Range(-3.5f, 3.5f);
            }
            else if (randomIndex == 0)
            {
                spawnY = Random.Range(0, 2) == 0 ? 2.5f : -2.5f;
            }
            else
            {
                spawnY = 0f;
            }

            Vector3 spawnPos = new Vector3(transform.position.x, spawnY, 0);
            Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

            timer = 0f;
        }
    }
}