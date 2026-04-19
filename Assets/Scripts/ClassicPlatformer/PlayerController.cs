using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Detection - Raycast")]
        [SerializeField] private float _raycastDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Attack - OverlapCircle")]
        [SerializeField] private float _attackRadius = 1.5f;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Fall Death")]
        [SerializeField] private float _fallDeathY = -10f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Rigidbody2D _rb;
        private PlayerHealth _health;
        private Animator _animator;
        private bool _isGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _health = GetComponent<PlayerHealth>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnDeath;
        }

        private void Update()
        {
            CheckGrounded();
            HandleJump();
            HandleAttack();
            CheckFallDeath();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxisRaw("Horizontal");
            _rb.linearVelocity = new Vector2(h * _moveSpeed, _rb.linearVelocity.y);

            if (_spriteRenderer != null && h != 0)
                _spriteRenderer.flipX = h < 0;
        }

        // Physics2D.Raycast вниз для перевірки isGrounded
        private void CheckGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                Vector2.down,
                _raycastDistance + 0.5f,
                _groundLayer);

            _isGrounded = hit.collider != null;
        }

        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
                _animator.SetTrigger("Jump");
            }
        }

        // OverlapCircle для атаки клавішею E
        private void HandleAttack()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                _attackRadius,
                _enemyLayer);

            foreach (var col in hits)
            {
                if (col.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage();
            }
        }

        private void CheckFallDeath()
        {
            if (transform.position.y < _fallDeathY)
                _health.InstantKill();
        }

        private void UpdateAnimator()
        {
            _animator.SetBool("isMoving", Mathf.Abs(_rb.linearVelocity.x) > 0.1f);
            _animator.SetBool("isGrounded", _isGrounded);
        }

        // Викликається з PlayerHealth.OnDeath
        public void TriggerHurtAnim()  => _animator.SetTrigger("Hurt");
        private void OnDeath()         => _animator.SetTrigger("Death");

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position,
                transform.position + Vector3.down * (_raycastDistance + 0.5f));

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
        }
    }
}
