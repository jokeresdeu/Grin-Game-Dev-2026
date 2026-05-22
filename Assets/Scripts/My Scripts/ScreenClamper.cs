using UnityEngine;

/// <summary>
/// Keeps object within screen boundaries.
/// Prevents object from leaving the visible area.
/// </summary>
public class ScreenClamper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool clampHorizontal = true;
    [SerializeField] private bool clampVertical = true;
    [SerializeField] private float padding = 0.5f;
    
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
    }
    
    private void CalculateScreenBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
        
        minX = bottomLeft.x + padding;
        maxX = topRight.x - padding;
        minY = bottomLeft.y + padding;
        maxY = topRight.y - padding;
    }
    
    private void LateUpdate()
    {
        Vector3 position = transform.position;
        
        if (clampHorizontal)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
        }
        
        if (clampVertical)
        {
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }
        
        transform.position = position;
    }
}
