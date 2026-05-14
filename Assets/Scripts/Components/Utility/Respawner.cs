using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles respawning at a checkpoint or start position.
/// Attach to player and call Respawn() when needed.
/// </summary>
public class Respawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool useStartPosition = true;
    [SerializeField] private float respawnDelay = 0f;
    
    [Header("Reset Options")]
    [SerializeField] private bool resetVelocity = true;
    [SerializeField] private bool resetHealth = true;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onRespawn;
    
    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Health health;
    
    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }
    
    public void SetCheckpoint(Transform checkpoint)
    {
        spawnPoint = checkpoint;
        useStartPosition = false;
    }
    
    public void SetCheckpoint(Vector3 position)
    {
        startPosition = position;
        useStartPosition = true;
        spawnPoint = null;
    }
    
    public void Respawn()
    {
        if (respawnDelay > 0f)
        {
            Invoke(nameof(DoRespawn), respawnDelay);
        }
        else
        {
            DoRespawn();
        }
    }
    
    private void DoRespawn()
    {
        Vector3 respawnPosition = useStartPosition || spawnPoint == null 
            ? startPosition 
            : spawnPoint.position;
        
        transform.position = respawnPosition;
        
        if (resetVelocity && rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        if (resetHealth && health != null)
        {
            health.ResetHealth();
        }
        
        onRespawn?.Invoke();
    }
}
