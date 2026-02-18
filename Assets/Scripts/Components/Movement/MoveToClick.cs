using UnityEngine;

/// <summary>
/// Moves object to the position where player clicks.
/// Great for point-and-click style movement.
/// </summary>
public class MoveToClick : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private int mouseButton = 0;
    
    [Header("Optional")]
    [SerializeField] private bool flipSprite = true;
    [SerializeField] private bool showTargetIndicator = false;
    [SerializeField] private GameObject targetIndicatorPrefab;
    
    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isMoving;
    private SpriteRenderer spriteRenderer;
    private GameObject currentIndicator;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            isMoving = true;
            
            if (showTargetIndicator && targetIndicatorPrefab != null)
            {
                if (currentIndicator != null) Destroy(currentIndicator);
                currentIndicator = Instantiate(targetIndicatorPrefab, targetPosition, Quaternion.identity);
            }
        }
        
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            
            if (flipSprite && spriteRenderer != null && direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
            
            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
            {
                isMoving = false;
                if (currentIndicator != null) Destroy(currentIndicator);
            }
        }
    }
}
