using UnityEngine;

/// <summary>
/// Automatically moves object in a specified direction.
/// Great for projectiles, scrolling backgrounds, or auto-runners.
/// </summary>
public class AutoMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool useLocalDirection = false;
    
    private void Update()
    {
        Vector3 moveDirection;
        
        if (useLocalDirection)
        {
            moveDirection = transform.TransformDirection(direction.normalized);
        }
        else
        {
            moveDirection = direction.normalized;
        }
        
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
