using UnityEngine;

/// <summary>
/// Компонент птаха. Рухається в заданому напрямку, має різні типи
/// (звичайний, швидкий, бонусний). Дає очки при знищенні.
/// При виході за межі екрану — гравець втрачає життя (Trigger).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bird : MonoBehaviour
{
    public enum BirdType
    {
        Normal,     // 1 очко
        Fast,       // 2 очки, швидший
        Bonus       // 3 очки, ще швидший
    }

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private Vector2 moveDirection = Vector2.right;

    [Header("Bird Type")]
    [SerializeField] private BirdType birdType = BirdType.Normal;

    [Header("Score")]
    [SerializeField] private int normalScore = 1;
    [SerializeField] private int fastScore = 2;
    [SerializeField] private int bonusScore = 3;

    [Header("Lifetime")]
    [SerializeField] private float maxLifetime = 10f;

    private Rigidbody2D _rb;
    private float _speed;
    private bool _isDead;

    public BirdType Type => birdType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        _speed = birdType switch
        {
            BirdType.Fast => baseSpeed * 1.6f,
            BirdType.Bonus => baseSpeed * 2.0f,
            _ => baseSpeed
        };

        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        if (_isDead) return;
        transform.Translate(moveDirection.normalized * _speed * Time.deltaTime);
    }

    /// <summary>
    /// Викликається при влучанні (OverlapCircle або Raycast).
    /// </summary>
    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        int points = birdType switch
        {
            BirdType.Fast => fastScore,
            BirdType.Bonus => bonusScore,
            _ => normalScore
        };

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(points);

        Destroy(gameObject);
    }

    /// <summary>
    /// Задає напрямок руху (використовується спавнером).
    /// </summary>
    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir;
        // Розвертаємо спрайт якщо рухається вліво
        if (dir.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
}
