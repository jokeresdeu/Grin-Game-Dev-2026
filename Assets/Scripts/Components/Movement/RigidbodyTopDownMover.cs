using UnityEngine;

/// <summary>
/// Moves Rigidbody2D in all directions using WASD or Arrow keys.
/// Uses physics for smooth movement with collision detection.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyTopDownMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool normalizeMovement = true;
    
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }
    
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(horizontal, vertical);
        
        if (normalizeMovement && movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        rb.linearVelocity = movement * speed;
    }
}
