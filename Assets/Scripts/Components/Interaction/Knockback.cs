using UnityEngine;

/// <summary>
/// Applies knockback force when this object hits something.
/// Great for attacks, explosions, or bumping into things.
/// </summary>
public class Knockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private LayerMask targetLayers = -1;
    
    [Header("Direction")]
    [SerializeField] private KnockbackDirection direction = KnockbackDirection.FromCenter;
    [SerializeField] private Vector2 fixedDirection = Vector2.right;
    
    [Header("Trigger")]
    [SerializeField] private bool useTrigger = true;
    [SerializeField] private bool useCollision = true;
    
    public enum KnockbackDirection
    {
        FromCenter,
        FixedDirection,
        MovementDirection
    }
    
    private Vector2 lastMovement;
    private Vector3 lastPosition;
    
    private void Start()
    {
        lastPosition = transform.position;
    }
    
    private void Update()
    {
        Vector3 movement = transform.position - lastPosition;
        if (movement.magnitude > 0.01f)
        {
            lastMovement = movement.normalized;
        }
        lastPosition = transform.position;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        ApplyKnockback(other.gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useCollision) return;
        ApplyKnockback(collision.gameObject);
    }
    
    private void ApplyKnockback(GameObject target)
    {
        if (!IsInTargetLayer(target)) return;
        
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb == null) return;
        
        Vector2 knockbackDir = GetKnockbackDirection(target.transform.position);
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
    }
    
    private Vector2 GetKnockbackDirection(Vector3 targetPosition)
    {
        switch (direction)
        {
            case KnockbackDirection.FixedDirection:
                return fixedDirection.normalized;
                
            case KnockbackDirection.MovementDirection:
                return lastMovement;
                
            case KnockbackDirection.FromCenter:
            default:
                return ((Vector2)(targetPosition - transform.position)).normalized;
        }
    }
    
    private bool IsInTargetLayer(GameObject target)
    {
        return (targetLayers.value & (1 << target.layer)) != 0;
    }
}
