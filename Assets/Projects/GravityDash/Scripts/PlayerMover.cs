using UnityEngine;

/// <summary>
/// Контролер гравця: горизонтальний рух + стрибок.
/// Використовує RigidbodyHorizontalMover та RigidbodyJumper з основного фреймворку.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [Header("Рух")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private bool _flipSprite = true;

    [Header("Стрибок")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    [Header("Перевірка землі")]
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Transform _groundCheck;
    private bool _isGrounded;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Автоматично створюємо точку перевірки землі
        GameObject checkObj = new GameObject("GroundCheck");
        checkObj.transform.SetParent(transform);
        checkObj.transform.localPosition = new Vector3(0f, -0.55f, 0f);
        _groundCheck = checkObj.transform;
    }

    void Update()
    {
        // Перевіряємо чи гравець стоїть на землі
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

        // Стрибок
        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Горизонтальний рух через фізику
        float horizontal = Input.GetAxis("Horizontal");
        _rigidbody.linearVelocity = new Vector2(horizontal * _speed, _rigidbody.linearVelocity.y);

        // Фліп спрайту
        if (_flipSprite && _spriteRenderer != null && horizontal != 0)
        {
            _spriteRenderer.flipX = horizontal < 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
