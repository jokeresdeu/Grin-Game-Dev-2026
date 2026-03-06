using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Detection")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rb;
        private float _horizontalInput;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rb.linearVelocity.y);

            if (_spriteRenderer != null && _horizontalInput != 0)
                _spriteRenderer.flipX = _horizontalInput < 0;
        }

        private void Jump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck != null)
            {
                Gizmos.color = _isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }
    }
}
