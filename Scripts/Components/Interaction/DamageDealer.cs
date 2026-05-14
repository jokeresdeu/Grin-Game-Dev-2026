using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Deals damage to objects with Health component on collision or trigger.
/// Use for projectiles, hazards, enemies, or anything that should hurt.
/// </summary>
public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool destroyOnHit = false;
    
    [Header("Target")]
    [SerializeField] private LayerMask targetLayers = -1;
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useCollision = false;
    
    [Header("Cooldown")]
    [SerializeField] private float damageCooldown = 0f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onDealDamage;
    
    private float lastDamageTime;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        TryDealDamage(other.gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!useTrigger || damageCooldown <= 0f) return;
        TryDealDamage(other.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useCollision) return;
        TryDealDamage(collision.gameObject);
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!useCollision || damageCooldown <= 0f) return;
        TryDealDamage(collision.gameObject);
    }
    
    private void TryDealDamage(GameObject target)
    {
        if (!IsInTargetLayer(target)) return;
        
        if (damageCooldown > 0f && Time.time < lastDamageTime + damageCooldown)
        {
            return;
        }
        
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            lastDamageTime = Time.time;
            
            onDealDamage?.Invoke();
            
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private bool IsInTargetLayer(GameObject target)
    {
        return (targetLayers.value & (1 << target.layer)) != 0;
    }
    
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
