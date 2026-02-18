using UnityEngine;

/// <summary>
/// Makes this object follow a target transform.
/// Useful for cameras, enemies, or companion objects.
/// </summary>
public class FollowTarget : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Follow Settings")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private Vector3 offset = Vector3.zero;
    
    [Header("Axis Lock")]
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        
        if (!followX) targetPosition.x = transform.position.x;
        if (!followY) targetPosition.y = transform.position.y;
        targetPosition.z = transform.position.z;
        
        if (smoothFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
