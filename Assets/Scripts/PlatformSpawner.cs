using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject spikePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float platformY = -3f;
    [SerializeField] private float platformHeight = 0.4f;
    [SerializeField] private float startX = -8f;
    [SerializeField] private float spawnAheadDistance = 45f;
    [SerializeField] private float destroyBehindDistance = 25f;

    [Header("Platform Size")]
    [SerializeField] private float minWidth = 4f;
    [SerializeField] private float maxWidth = 9f;

    [Header("Gameplay")]
    [SerializeField] private float overlap = 1.5f;
    [SerializeField] private bool alternateModes = true;

    [Header("Obstacle Settings")]
    [SerializeField] private float coinSpawnChance = 0.65f;
    [SerializeField] private float spikeSpawnChance = 0.45f;
    [SerializeField] private float objectY = -2.35f;

    [Header("Colors")]
    [SerializeField] private Color lightPlatformColor = new Color(1f, 0.9f, 0.3f);
    [SerializeField] private Color darkPlatformColor = new Color(0.45f, 0.25f, 1f);

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    private float nextStartX;
    private WorldMode nextMode = WorldMode.Light;
    private int platformCount;

    private void Start()
    {
        nextStartX = startX;

        SpawnPlatform(12f, WorldMode.Light, false);

        while (player != null && nextStartX < player.position.x + spawnAheadDistance)
        {
            SpawnRandomPlatform();
        }
    }

    private void Update()
    {
        if (player == null || platformPrefab == null)
        {
            return;
        }

        while (nextStartX < player.position.x + spawnAheadDistance)
        {
            SpawnRandomPlatform();
        }

        DestroyOldObjects();
    }

    private void SpawnRandomPlatform()
    {
        float randomWidth = Random.Range(minWidth, maxWidth);

        WorldMode mode;

        if (alternateModes)
        {
            mode = nextMode;
            nextMode = nextMode == WorldMode.Light ? WorldMode.Dark : WorldMode.Light;
        }
        else
        {
            mode = Random.value > 0.5f ? WorldMode.Light : WorldMode.Dark;
        }

        SpawnPlatform(randomWidth, mode, true);
    }

    private void SpawnPlatform(float width, WorldMode mode, bool canSpawnObjects)
    {
        Vector3 spawnPosition = new Vector3(nextStartX + width / 2f, platformY, 0f);

        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platform.name = mode + "_Platform";
        platform.transform.localScale = new Vector3(width, platformHeight, 1f);

        ModeObject modeObject = platform.GetComponent<ModeObject>();

        if (modeObject != null)
        {
            modeObject.SetActiveMode(mode);
        }

        SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = mode == WorldMode.Light ? lightPlatformColor : darkPlatformColor;
        }

        spawnedObjects.Add(platform);

        if (canSpawnObjects && platformCount > 1)
        {
            SpawnCoinOrSpike(width, mode);
        }

        platformCount++;
        nextStartX += width - overlap;

        if (WorldModeManager.Instance != null)
        {
            WorldModeManager.Instance.RefreshMode();
        }
    }

    private void SpawnCoinOrSpike(float platformWidth, WorldMode mode)
    {
        float platformCenterX = nextStartX + platformWidth / 2f;

        if (coinPrefab != null && Random.value < coinSpawnChance)
        {
            float coinX = platformCenterX + Random.Range(-platformWidth * 0.25f, platformWidth * 0.25f);
            Vector3 coinPosition = new Vector3(coinX, objectY + 0.8f, 0f);

            GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            spawnedObjects.Add(coin);
        }

        if (spikePrefab != null && Random.value < spikeSpawnChance)
        {
            float spikeX = platformCenterX + Random.Range(-platformWidth * 0.25f, platformWidth * 0.25f);
            Vector3 spikePosition = new Vector3(spikeX, objectY, 0f);

            GameObject spike = Instantiate(spikePrefab, spikePosition, Quaternion.identity);

            ModeObject spikeModeObject = spike.GetComponent<ModeObject>();

            if (spikeModeObject != null)
            {
                spikeModeObject.SetActiveMode(mode);
            }

            spawnedObjects.Add(spike);
        }
    }

    private void DestroyOldObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedObjects[i];

            if (obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (obj.transform.position.x < player.position.x - destroyBehindDistance)
            {
                spawnedObjects.RemoveAt(i);
                Destroy(obj);
            }
        }
    }
}