using UnityEngine;

/// <summary>
/// Moves object along a series of waypoints.
/// Great for enemies, platforms, or any patrolling behavior.
/// </summary>
public class PatrolPath : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool pingPong = false;
    
    [Header("Optional")]
    [SerializeField] private bool flipSprite = true;
    
    private int currentWaypointIndex;
    private float waitCounter;
    private bool isWaiting;
    private bool movingForward = true;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWaiting = false;
            }
            return;
        }
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        
        if (flipSprite && spriteRenderer != null && direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            MoveToNextWaypoint();
        }
    }
    
    private void MoveToNextWaypoint()
    {
        isWaiting = true;
        waitCounter = waitTime;
        
        if (pingPong)
        {
            if (movingForward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = waypoints.Length - 2;
                    movingForward = false;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 1;
                    movingForward = true;
                }
            }
        }
        else if (loop)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else
        {
            currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            
            Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);
            
            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
        
        if (loop && waypoints.Length > 1 && waypoints[0] != null && waypoints[waypoints.Length - 1] != null)
        {
            Gizmos.color = Color.yellow * 0.5f;
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
}
