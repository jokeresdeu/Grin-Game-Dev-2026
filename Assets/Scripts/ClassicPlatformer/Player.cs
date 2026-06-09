using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
 
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
 
        [Header("Animation")]
        [SerializeField] private Animator _animator;
 
        [Header("Health")]
        [SerializeField] private int _maxHealth = 3;
 
        [Header("Invincibility")]
        [SerializeField] private float _invincibilityDuration = 1.5f;
 
        [Header("Death")]
        [SerializeField] private float _deathDelay = 1f; 
 
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _healthText;
 
        private int _currentHealth;
        private float _invincibilityTimer;
        private bool _isInvincible;
        private bool _isDead = false;
 
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
 
        private Rigidbody2D _rb;
        private float _horizontalInput;
        private float _verticalMovement;
        private bool _isGrounded;
        private bool _verticalMovementEnabled;
 
        private float _blinkTimer;
        private float _blinkInterval = 0.1f;
 
        private void Awake()
        {
            _currentHealth = _maxHealth;
            _rb = GetComponent<Rigidbody2D>();
 
            if (_animator == null)
                _animator = GetComponent<Animator>();
 
            UpdateUI();
        }
 
        private void Update()
        {
            if (_isDead) return;
 
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");
 
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
 
            if (Input.GetButtonDown("Jump") && _isGrounded)
                Jump();
 
            HandleInvincibility();
            UpdateAnimator();
        }
 
        private void FixedUpdate()
        {
            if (_isDead) return;
 
            float velocityY = _verticalMovementEnabled
                ? _verticalMovement * _climbSpeed
                : _rb.linearVelocity.y;
 
            _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, velocityY);
 
            if (_spriteRenderer != null && _horizontalInput != 0)
                _spriteRenderer.flipX = _horizontalInput < 0;
        }
 
        private void UpdateAnimator()
        {
            if (_animator == null) return;
 
            _animator.SetFloat("Speed", Mathf.Abs(_horizontalInput));
            _animator.SetBool("IsGrounded", _isGrounded);
        }
 
        public void EnableVerticalMovement(bool enabled)
        {
            _rb.bodyType = enabled ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            _verticalMovementEnabled = enabled;
        }
 
        private void Jump()
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }
 
        private void HandleInvincibility()
        {
            if (!_isInvincible) return;
 
            _invincibilityTimer -= Time.deltaTime;
            _blinkTimer -= Time.deltaTime;
 
            if (_blinkTimer <= 0f)
            {
                _blinkTimer = _blinkInterval;
                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = !_spriteRenderer.enabled;
            }
 
            if (_invincibilityTimer <= 0f)
            {
                _isInvincible = false;
                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = true;
            }
        }
 
        public void TakeDamage(int damage = 1)
        {
            if (_isInvincible || _currentHealth <= 0 || _isDead) return;
 
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(_currentHealth, 0);
            UpdateUI();
 
            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                _isInvincible = true;
                _invincibilityTimer = _invincibilityDuration;
                _blinkTimer = _blinkInterval;
            }
        }
 
        public void Heal(int amount = 1)
        {
            if (_currentHealth <= 0) return;
 
            _currentHealth += amount;
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
            UpdateUI();
        }
 
        private void Die()
        {
            _isDead = true;

            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            if (_animator != null)
                _animator.SetTrigger("Death");
 
            StartCoroutine(RestartAfterDeath());
        }
 
        private IEnumerator RestartAfterDeath()
        {
            yield return new WaitForSeconds(_deathDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
 
        private void UpdateUI()
        {
            if (_healthText != null)
                _healthText.text = $"HP: {_currentHealth}/{_maxHealth}";
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