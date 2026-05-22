using UnityEngine;

/// <summary>
/// Makes Rigidbody2D jump when pressing the jump button (Space).
/// Includes ground detection for proper platformer jumping.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyJumper : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.SetParent(transform);
            check.transform.localPosition = new Vector3(0f, -0.5f, 0f);
            groundCheck = check.transform;
        }
    }
    
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }
    
    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
