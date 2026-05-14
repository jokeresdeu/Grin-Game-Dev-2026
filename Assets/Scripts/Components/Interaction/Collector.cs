using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collects Collectible objects on contact.
/// Attach to player or any object that should pick things up.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{
    [Header("Collection Settings")]
    [SerializeField] private string[] collectibleTypes;
    [SerializeField] private bool collectAllTypes = true;
    
    [Header("Events")]
    [SerializeField] private UnityEvent<Collectible> onCollect;
    [SerializeField] private UnityEvent<int> onValueCollected;
    
    private int totalCollected;
    
    public int TotalCollected => totalCollected;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryCollect(other.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryCollect(collision.gameObject);
    }
    
    private void TryCollect(GameObject target)
    {
        Collectible collectible = target.GetComponent<Collectible>();
        if (collectible == null) return;
        
        if (!collectAllTypes && !IsValidType(collectible.CollectibleType)) return;
        
        collectible.Collect(gameObject);
        totalCollected += collectible.Value;
        
        onCollect?.Invoke(collectible);
        onValueCollected?.Invoke(collectible.Value);
    }
    
    private bool IsValidType(string type)
    {
        if (collectibleTypes == null || collectibleTypes.Length == 0) return true;
        
        foreach (string validType in collectibleTypes)
        {
            if (validType == type) return true;
        }
        
        return false;
    }
    
    public void ResetCollection()
    {
        totalCollected = 0;
    }
}
