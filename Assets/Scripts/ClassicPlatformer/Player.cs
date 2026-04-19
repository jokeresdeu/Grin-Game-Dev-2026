using System.Collections;
using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _climbSpeed = 3.5f;
        [SerializeField] private float _jumpForce = 14f;

        [Header("Ground Detection")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Animator animator;

        [Header("Health")]
        [SerializeField] private int _maxHealth = 3;

        [Header("Invincibility")]
        [SerializeField] private float _invincibilityDuration = 1.5f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _healthText;

        [Header("Combat")]
        [SerializeField] private float _attackDuration = 0.4f;
        private bool _isAttacking;

        [Header("Death")]
        [SerializeField] private float _deathDelay = 2f;
        private bool _isDead;

        private bool _wasGrounded;
        private int _currentHealth;
        private float _invincibilityTimer;
        private bool _isInvincible;

        private Rigidbody2D _rb;
        private float _horizontalInput;
        private float _verticalMovement;
        private bool _isGrounded;
        private bool _verticalMovementEnabled;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _rb = GetComponent<Rigidbody2D>();
            UpdateUI();
        }

        private void Update()
        {
            if (_isDead) return;
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            if (this.transform.position.y <= -4f) EnableVerticalMovement(false);
            if (!_wasGrounded && _isGrounded)
                animator.SetBool("Grounded", true);
            if (_wasGrounded && !_isGrounded)
                animator.SetBool("Grounded", false);
            _wasGrounded = _isGrounded;
            if (Input.GetMouseButtonDown(0) && !_isAttacking)
            {
                StartCoroutine(Attack());
            }

            if (Input.GetButtonDown("Jump") && _isGrounded && !_isAttacking && !_verticalMovementEnabled)
            {
                Jump();
            }

            if (_isInvincible)
            {
                _invincibilityTimer -= Time.deltaTime;
                if (_invincibilityTimer <= 0f)
                    _isInvincible = false;
            }
            animator.SetFloat("AirSpeedY", _rb.linearVelocity.y);
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            float velocityY = _verticalMovementEnabled? _verticalMovement * _climbSpeed: _rb.linearVelocity.y;

            if (_isAttacking && !_verticalMovementEnabled)
            {
                _rb.linearVelocity = new Vector2(0, velocityY);
                return;
            }

            _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, velocityY);

            if (_spriteRenderer != null && _horizontalInput != 0)
                _spriteRenderer.flipX = _horizontalInput < 0;

            if (Mathf.Abs(_horizontalInput) > 0.01f)
                animator.SetInteger("AnimState", 1);
            else
                animator.SetInteger("AnimState", 0);
        }

        public void EnableVerticalMovement(bool enabled)
        {
            if (_isDead) return;
            _verticalMovementEnabled = enabled;
            if (!_isAttacking)
            {
                _rb.bodyType = enabled ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            }
        }

        private void Jump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);

            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
        }
        private IEnumerator Attack()
        {
            _isAttacking = true;
            animator.SetTrigger("Attack1");

            yield return new WaitForSeconds(_attackDuration);

            _isAttacking = false;
        }

        public void TakeDamage(int damage = 1)
        {
            if (_isInvincible || _currentHealth <= 0 || _isDead) return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            UpdateUI();

            animator.SetTrigger("Hurt");

            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                _isInvincible = true;
                _invincibilityTimer = _invincibilityDuration;
            }
        }

        private void Die()
        {
            _isDead = true;

            animator.SetTrigger("Death");

            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(_deathDelay);
            Destroy(gameObject);
        }

        public void Heal(int amount = 1)
        {
            if (_currentHealth <= 0) return;

            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_healthText != null)
                _healthText.text = $"HP: {_currentHealth}/{_maxHealth}";
        }
    }
}