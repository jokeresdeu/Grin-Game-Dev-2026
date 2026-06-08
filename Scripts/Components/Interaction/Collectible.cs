using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Makes this object collectible by objects with Collector component.
/// Use for coins, power-ups, health pickups, keys, etc.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private string collectibleType = "Coin";
    [SerializeField] private int value = 1;
    
    [Header("Collection")]
    [SerializeField] private bool destroyOnCollect = true;
    [SerializeField] private float collectDelay = 0f;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject collectEffectPrefab;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onCollected;
    
    private bool isCollected;
    
    public string CollectibleType => collectibleType;
    public int Value => value;
    
    public void Collect(GameObject collector)
    {
        if (isCollected) return;
        isCollected = true;
        
        onCollected?.Invoke();
        
        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }
        
        if (destroyOnCollect)
        {
            if (collectDelay > 0f)
            {
                Destroy(gameObject, collectDelay);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
