using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Heals objects with Health component on contact.
/// Use for health pickups, healing zones, or support abilities.
/// </summary>
public class Healer : MonoBehaviour
{
    [Header("Heal Settings")]
    [SerializeField] private float healAmount = 25f;
    [SerializeField] private bool destroyOnHeal = true;
    
    [Header("Target")]
    [SerializeField] private LayerMask targetLayers = -1;
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useCollision = false;
    
    [Header("Continuous Healing")]
    [SerializeField] private bool continuousHeal = false;
    [SerializeField] private float healInterval = 1f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onHeal;
    
    private float lastHealTime;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        TryHeal(other.gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!useTrigger || !continuousHeal) return;
        if (Time.time < lastHealTime + healInterval) return;
        TryHeal(other.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useCollision) return;
        TryHeal(collision.gameObject);
    }
    
    private void TryHeal(GameObject target)
    {
        if (!IsInTargetLayer(target)) return;
        
        Health health = target.GetComponent<Health>();
        if (health != null && health.CurrentHealth < health.MaxHealth)
        {
            health.Heal(healAmount);
            lastHealTime = Time.time;
            
            onHeal?.Invoke();
            
            if (destroyOnHeal)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private bool IsInTargetLayer(GameObject target)
    {
        return (targetLayers.value & (1 << target.layer)) != 0;
    }
}
