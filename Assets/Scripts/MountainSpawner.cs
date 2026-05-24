using UnityEngine;

public class MountainSpawner : MonoBehaviour
{
    public GameObject[] bottomMountains;
    public GameObject[] topMountains;

    public float minSpawnRate = 1.5f;
    public float maxSpawnRate = 3.5f;

    public float groundY = -3.5f;
    public float topY = 4.5f;

    private float bottomTimer;
    private float topTimer;

    private float nextBottomSpawnDelay;
    private float nextTopSpawnDelay;

    private PlaneController plane;

    void Start()
    {
        plane = FindFirstObjectByType<PlaneController>();

        float firstSpawnX = transform.position.x + 9f;

        if (bottomMountains.Length > 0)
        {
            int randomB = Random.Range(0, bottomMountains.Length);
            Instantiate(bottomMountains[randomB], new Vector3(firstSpawnX, groundY, 0), Quaternion.identity);
        }

        if (topMountains.Length > 0)
        {
            int randomT = Random.Range(0, topMountains.Length);
            Instantiate(topMountains[randomT], new Vector3(firstSpawnX + 3f, topY, 0), topMountains[randomT].transform.rotation);
        }

        nextBottomSpawnDelay = Random.Range(minSpawnRate, maxSpawnRate);
        nextTopSpawnDelay = Random.Range(minSpawnRate, maxSpawnRate);
    }

    void Update()
    {
        if (plane != null && (!plane.gameStarted || plane.isDead)) return;

        bottomTimer += Time.deltaTime;
        topTimer += Time.deltaTime;

        float spawnX = transform.position.x + 15f;

        if (bottomTimer >= nextBottomSpawnDelay)
        {
            if (bottomMountains.Length > 0)
            {
                int randomB = Random.Range(0, bottomMountains.Length);
                Instantiate(bottomMountains[randomB], new Vector3(spawnX, groundY, 0), Quaternion.identity);
            }
            bottomTimer = 0f;
            nextBottomSpawnDelay = Random.Range(minSpawnRate, maxSpawnRate);
        }

        if (topTimer >= nextTopSpawnDelay)
        {
            if (topMountains.Length > 0)
            {
                int randomT = Random.Range(0, topMountains.Length);
                Instantiate(topMountains[randomT], new Vector3(spawnX, topY, 0), topMountains[randomT].transform.rotation);
            }
            topTimer = 0f;
            nextTopSpawnDelay = Random.Range(minSpawnRate, maxSpawnRate);
        }
    }
}