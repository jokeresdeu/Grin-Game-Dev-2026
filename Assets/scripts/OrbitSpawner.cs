using System.Collections.Generic;
using UnityEngine;

public class OrbitSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Orbit")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float radius = 2.5f;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float minSpawnInterval = 0.55f;
    [SerializeField] private float intervalDecreaseSpeed = 0.01f;
    [SerializeField] private int maxObjectsOnOrbit = 10;
    [SerializeField] private float minAngleDistance = 35f;
    [SerializeField] private float minAngleFromPlayer = 45f;
    [SerializeField] private float collectibleChance = 0.35f;

    private float timer;
    private Transform player;
    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        if (orbitCenter == null)
        {
            GameObject centerObject = GameObject.Find("OrbitCenter");
            if (centerObject != null)
                orbitCenter = centerObject.transform;
        }

        PlayerOrbit playerOrbit = FindFirstObjectByType<PlayerOrbit>();
        if (playerOrbit != null)
            player = playerOrbit.transform;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsPaused) return;

        timer += Time.deltaTime;

        spawnInterval = Mathf.Max(
            minSpawnInterval,
            spawnInterval - intervalDecreaseSpeed * Time.deltaTime
        );

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        spawnedObjects.RemoveAll(item => item == null);

        if (spawnedObjects.Count >= maxObjectsOnOrbit)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
        }

        bool spawnCollectible = Random.value < collectibleChance;
        GameObject prefab = spawnCollectible ? collectiblePrefab : obstaclePrefab;

        if (prefab == null || orbitCenter == null) return;

        float angle = GetSafeAngle();
        float radians = angle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(
            Mathf.Cos(radians),
            Mathf.Sin(radians),
            0f
        );

        Vector3 spawnPosition = orbitCenter.position + direction * radius;

        GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newObject.transform.up = direction.normalized;

        spawnedObjects.Add(newObject);
    }

    private float GetSafeAngle()
    {
        for (int i = 0; i < 30; i++)
        {
            float angle = Random.Range(0f, 360f);

            if (IsAngleSafe(angle))
                return angle;
        }

        return Random.Range(0f, 360f);
    }

    private bool IsAngleSafe(float angle)
    {
        if (player != null)
        {
            float playerAngle = GetAngleFromPosition(player.position);

            if (Mathf.Abs(Mathf.DeltaAngle(angle, playerAngle)) < minAngleFromPlayer)
                return false;
        }

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj == null) continue;

            float objectAngle = GetAngleFromPosition(obj.transform.position);

            if (Mathf.Abs(Mathf.DeltaAngle(angle, objectAngle)) < minAngleDistance)
                return false;
        }

        return true;
    }

    private float GetAngleFromPosition(Vector3 position)
    {
        Vector3 offset = position - orbitCenter.position;
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }
}