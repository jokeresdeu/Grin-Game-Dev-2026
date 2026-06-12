using UnityEngine;

/// <summary>
/// Allows the object to be dragged with the mouse.
/// Requires a Collider2D to detect mouse interaction.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool smoothDrag = true;
    [SerializeField] private float smoothSpeed = 20f;
    [SerializeField] private bool returnToStart = false;
    
    [Header("Constraints")]
    [SerializeField] private bool constrainX = false;
    [SerializeField] private bool constrainY = false;
    
    private Camera mainCamera;
    private bool isDragging;
    private Vector3 offset;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        startPosition = transform.position;
        targetPosition = transform.position;
    }
    
    private void OnMouseDown()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePosition;
        offset.z = 0f;
        isDragging = true;
    }
    
    private void OnMouseDrag()
    {
        if (!isDragging) return;
        
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = mousePosition + offset;
        targetPosition.z = transform.position.z;
        
        if (constrainX) targetPosition.x = transform.position.x;
        if (constrainY) targetPosition.y = transform.position.y;
    }
    
    private void OnMouseUp()
    {
        isDragging = false;
        
        if (returnToStart)
        {
            targetPosition = startPosition;
        }
    }
    
    private void Update()
    {
        if (smoothDrag)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else if (isDragging)
        {
            transform.position = targetPosition;
        }
    }
}
