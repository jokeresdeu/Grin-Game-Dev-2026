using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spawns a prefab when player presses a key.
/// Great for shooting, placing objects, or spawning effects.
/// </summary>
public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private KeyCode spawnKey = KeyCode.Space;
    
    [Header("Spawn Options")]
    [SerializeField] private bool inheritRotation = true;
    [SerializeField] private Vector3 spawnOffset = Vector3.zero;
    
    [Header("Cooldown")]
    [SerializeField] private float cooldown = 0.5f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onSpawn;
    
    private float lastSpawnTime;
    
    private void Awake()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(spawnKey) && Time.time >= lastSpawnTime + cooldown)
        {
            Spawn();
        }
    }
    
    public void Spawn()
    {
        if (prefab == null) return;
        
        Vector3 position = spawnPoint.position + spawnOffset;
        Quaternion rotation = inheritRotation ? spawnPoint.rotation : Quaternion.identity;
        
        Instantiate(prefab, position, rotation);
        lastSpawnTime = Time.time;
        
        onSpawn?.Invoke();
    }
}
