using UnityEngine;

/// <summary>
/// Simple 2D camera follow script.
/// Follows a target with smooth movement and optional bounds.
/// </summary>
public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    
    [Header("Axis Lock")]
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    
    [Header("Bounds (Optional)")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;
    
    [Header("Look Ahead")]
    [SerializeField] private bool lookAhead = false;
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSpeed = 3f;
    
    private Vector3 currentLookAhead;
    private Vector3 lastTargetPosition;
    
    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        
        if (lookAhead)
        {
            Vector3 moveDirection = (target.position - lastTargetPosition).normalized;
            Vector3 targetLookAhead = moveDirection * lookAheadDistance;
            currentLookAhead = Vector3.Lerp(currentLookAhead, targetLookAhead, lookAheadSpeed * Time.deltaTime);
            targetPosition += currentLookAhead;
            lastTargetPosition = target.position;
        }
        
        if (!followX) targetPosition.x = transform.position.x;
        if (!followY) targetPosition.y = transform.position.y;
        
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        lastTargetPosition = newTarget != null ? newTarget.position : transform.position;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;
        
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
