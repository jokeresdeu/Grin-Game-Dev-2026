using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRunner : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 0.15f;

    [Header("Death")]
    [SerializeField] private float deathY = -8f;

    private Rigidbody2D rb;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = startSpeed;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGameRunning)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        currentSpeed += acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, startSpeed, maxSpeed);

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    private void Update()
    {
        if (transform.position.y < deathY)
        {
            GameManager.Instance.GameOver();
        }
    }
}