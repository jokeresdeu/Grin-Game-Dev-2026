using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Automatically spawns prefabs at regular intervals.
/// Perfect for enemy spawners, particle effects, or item generators.
/// </summary>
public class TimedSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    
    [Header("Timing")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float initialDelay = 0f;
    [SerializeField] private bool spawnOnStart = false;
    
    [Header("Limits")]
    [SerializeField] private int maxSpawns = -1;
    [SerializeField] private int maxActiveObjects = -1;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onSpawn;
    
    private float timer;
    private int spawnCount;
    private bool hasStarted;
    
    private void Awake()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
    }
    
    private void Start()
    {
        timer = initialDelay;
        
        if (spawnOnStart && initialDelay <= 0f)
        {
            Spawn();
        }
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0f)
        {
            if (!hasStarted)
            {
                hasStarted = true;
                if (!spawnOnStart)
                {
                    Spawn();
                }
            }
            else
            {
                Spawn();
            }
            
            timer = spawnInterval;
        }
    }
    
    public void Spawn()
    {
        if (prefab == null) return;
        
        if (maxSpawns >= 0 && spawnCount >= maxSpawns) return;
        
        if (maxActiveObjects >= 0)
        {
            GameObject[] activeObjects = GameObject.FindGameObjectsWithTag(prefab.tag);
            if (activeObjects.Length >= maxActiveObjects) return;
        }
        
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        spawnCount++;
        
        onSpawn?.Invoke();
    }
    
    public void ResetSpawner()
    {
        spawnCount = 0;
        timer = initialDelay;
        hasStarted = false;
    }
}
