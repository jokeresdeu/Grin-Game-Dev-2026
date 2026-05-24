using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab;

    public float spawnRate = 2f;
    public float heightOffset = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnPipe();
            timer = 0f;
        }
    }

    void SpawnPipe()
    {
        float randomY = Random.Range(-heightOffset, heightOffset);

        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0f);

        Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
    }
}