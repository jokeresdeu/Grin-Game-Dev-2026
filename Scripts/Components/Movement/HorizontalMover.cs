using UnityEngine;

/// <summary>
/// Moves transform horizontally based on keyboard input (A/D or Arrow keys).
/// </summary>
public class HorizontalMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    
    [Header("Optional")]
    [SerializeField] private bool flipSprite = true;
    
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
        
        if (flipSprite && spriteRenderer != null && horizontal != 0)
        {
            spriteRenderer.flipX = horizontal < 0;
        }
    }
}
