using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead) return;

        if (Mouse.current.leftButton.wasPressedThisFrame || 
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame))
        {
            rb.linearVelocity = Vector2.up * jumpForce;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        isDead = true;

        GameManager.Instance.GameOver();
    }
}