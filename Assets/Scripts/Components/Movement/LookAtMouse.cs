using UnityEngine;

/// <summary>
/// Rotates object to face the mouse cursor.
/// Perfect for aiming weapons or turrets in 2D games.
/// </summary>
public class LookAtMouse : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rotationOffset = 0f;
    [SerializeField] private bool smoothRotation = false;
    [SerializeField] private float rotationSpeed = 10f;
    
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffset;
        
        if (smoothRotation)
        {
            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
        }
    }
}
