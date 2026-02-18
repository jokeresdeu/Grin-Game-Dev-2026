using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Destroys object when it collides with specified layers.
/// Use for projectiles hitting walls, breakable objects, etc.
/// </summary>
public class DestroyOnCollision : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private LayerMask destroyOnLayers = -1;
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useCollision = true;
    
    [Header("Effects")]
    [SerializeField] private GameObject destroyEffectPrefab;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onDestroy;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        TryDestroy(other.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useCollision) return;
        TryDestroy(collision.gameObject);
    }
    
    private void TryDestroy(GameObject target)
    {
        if ((destroyOnLayers.value & (1 << target.layer)) == 0) return;
        
        onDestroy?.Invoke();
        
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
