using UnityEngine;

/// <summary>
/// Moves Rigidbody2D horizontally based on keyboard input.
/// Uses physics for smooth movement with collision detection.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyHorizontalMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    
    [Header("Optional")]
    [SerializeField] private bool flipSprite = true;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        
        if (flipSprite && spriteRenderer != null && horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }
}
