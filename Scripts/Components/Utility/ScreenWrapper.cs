using UnityEngine;

/// <summary>
/// Wraps object around screen edges (like Asteroids).
/// When object exits one side, it appears on the opposite side.
/// </summary>
public class ScreenWrapper : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool wrapHorizontal = true;
    [SerializeField] private bool wrapVertical = true;
    [SerializeField] private float buffer = 0.5f;
    
    private Camera mainCamera;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
    }
    
    private void CalculateScreenBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
        
        screenLeft = bottomLeft.x - buffer;
        screenRight = topRight.x + buffer;
        screenBottom = bottomLeft.y - buffer;
        screenTop = topRight.y + buffer;
    }
    
    private void LateUpdate()
    {
        Vector3 position = transform.position;
        
        if (wrapHorizontal)
        {
            if (position.x < screenLeft)
            {
                position.x = screenRight - buffer;
            }
            else if (position.x > screenRight)
            {
                position.x = screenLeft + buffer;
            }
        }
        
        if (wrapVertical)
        {
            if (position.y < screenBottom)
            {
                position.y = screenTop - buffer;
            }
            else if (position.y > screenTop)
            {
                position.y = screenBottom + buffer;
            }
        }
        
        transform.position = position;
    }
}
