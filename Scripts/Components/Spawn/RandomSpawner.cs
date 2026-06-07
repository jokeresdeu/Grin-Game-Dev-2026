using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spawns prefabs at random positions within a defined area.
/// Great for spawning enemies, items, or environmental objects.
/// </summary>
public class RandomSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] prefabs;
    
    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 5f);
    [SerializeField] private Vector2 spawnAreaOffset = Vector2.zero;
    
    [Header("Timing")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float randomIntervalVariance = 0.5f;
    
    [Header("Limits")]
    [SerializeField] private int maxActiveObjects = 10;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onSpawn;
    
    private float timer;
    private int activeCount;
    
    private void Start()
    {
        ResetTimer();
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            TrySpawn();
            ResetTimer();
        }
    }
    
    private void ResetTimer()
    {
        timer = spawnInterval + Random.Range(-randomIntervalVariance, randomIntervalVariance);
    }
    
    public void TrySpawn()
    {
        if (prefabs == null || prefabs.Length == 0) return;
        
        activeCount = 0;
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
            {
                activeCount += GameObject.FindGameObjectsWithTag(prefab.tag).Length;
            }
        }
        
        if (maxActiveObjects >= 0 && activeCount >= maxActiveObjects) return;
        
        Vector3 randomPosition = GetRandomPosition();
        GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Length)];
        
        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
            onSpawn?.Invoke();
        }
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position + (Vector3)spawnAreaOffset;
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        
        return center + new Vector3(randomX, randomY, 0f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + (Vector3)spawnAreaOffset;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0.1f));
    }
}
