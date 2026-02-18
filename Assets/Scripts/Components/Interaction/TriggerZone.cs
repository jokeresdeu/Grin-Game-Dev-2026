using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fires events when objects enter, stay in, or exit a trigger zone.
/// Great for checkpoints, spawn triggers, cutscene triggers, or area effects.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TriggerZone : MonoBehaviour
{
    [Header("Filter")]
    [SerializeField] private bool filterByTag = true;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private LayerMask targetLayers = -1;
    
    [Header("Trigger Settings")]
    [SerializeField] private bool oneTimeTrigger = false;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onStay;
    [SerializeField] private UnityEvent onExit;
    
    private bool hasTriggered;
    private int objectsInZone;
    
    public bool HasTriggered => hasTriggered;
    public int ObjectsInZone => objectsInZone;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidTarget(other.gameObject)) return;
        if (oneTimeTrigger && hasTriggered) return;
        
        objectsInZone++;
        hasTriggered = true;
        onEnter?.Invoke();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsValidTarget(other.gameObject)) return;
        if (oneTimeTrigger && hasTriggered) return;
        
        onStay?.Invoke();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsValidTarget(other.gameObject)) return;
        
        objectsInZone = Mathf.Max(0, objectsInZone - 1);
        onExit?.Invoke();
    }
    
    private bool IsValidTarget(GameObject target)
    {
        if (filterByTag && !target.CompareTag(targetTag)) return false;
        if ((targetLayers.value & (1 << target.layer)) == 0) return false;
        return true;
    }
    
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
