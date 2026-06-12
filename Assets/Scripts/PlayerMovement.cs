using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 6f;
    public float horizontalSpeed = 3f;
    public float speedIncreaseRate = 0.3f;

    private Rigidbody2D rb;
    private int direction = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalSpeed * direction, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void ReverseDirectionAndAccelerate()
    {
        direction *= -1;
        horizontalSpeed += speedIncreaseRate;
    }
}