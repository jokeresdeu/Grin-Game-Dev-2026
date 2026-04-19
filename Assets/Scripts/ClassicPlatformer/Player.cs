using Platformer;
using System.Collections;
using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour, IDamageable
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 7f;
        [SerializeField] private float _climbSpeed = 3.5f;
        [SerializeField] private float _jumpForce = 14f;
        [SerializeField] private float _rollForce = 8f;
        [SerializeField] private float _rollDuration = 8f / 14f;
        [SerializeField] private PlayerView _playerView;

        [Header("Ground Detection")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Wall Detection")]
        [SerializeField] private Transform _wallCheckRight1;
        [SerializeField] private Transform _wallCheckRight2;
        [SerializeField] private Transform _wallCheckLeft1;
        [SerializeField] private Transform _wallCheckLeft2;
        [SerializeField] private float _wallCheckRadius = 0.1f;

        [Header("Combat")]
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRange = 1.0f;
        [SerializeField] private int _attackDamage = 1;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Health")]
        [SerializeField] private int _maxHealth = 3;

        [Header("Experience")]
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private int _currentExperience = 0;
        [SerializeField] private int _experienceToNextLevel = 50;

        [Header("Invincibility")]
        [SerializeField] private float _invincibilityDuration = 1.5f;

        [Header("Death")]
        [SerializeField] private float _deathAnimationDuration = 1.5f;
        [SerializeField] private GameObject _deathMenu;

        private int _currentHealth;
        private float _invincibilityTimer;
        private bool _isInvincible;
        private bool _isRolling;
        private bool _isWallSliding;
        private bool _isDead;
        private bool _verticalMovementEnabled;

        private int _facingDirection = 1;
        private int _currentAttack = 0;
        private float _timeSinceAttack = 0f;
        private float _delayToIdle = 0f;

        private Rigidbody2D _rb;
        private Animator _animator;

        private float _horizontalInput;
        private float _verticalMovement;
        private bool _isGrounded;
        private bool _wasGrounded;

        private float _rollCurrentTime;
        private Coroutine _deathCoroutine;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public int CurrentLevel => _currentLevel;
        public int CurrentExperience => _currentExperience;
        public int ExperienceToNextLevel => _experienceToNextLevel;

        private void Awake()
        {
            Time.timeScale = 1f;

            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.simulated = true;
            _rb.linearVelocity = Vector2.zero;

            _currentHealth = _maxHealth;

            _isInvincible = false;
            _isRolling = false;
            _isWallSliding = false;
            _isDead = false;
            _verticalMovementEnabled = false;

            _rollCurrentTime = 0f;
            _timeSinceAttack = 0f;
            _delayToIdle = 0f;
            _currentAttack = 0;

            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
            _wasGrounded = _isGrounded;

            _animator.SetBool("Grounded", _isGrounded);
            _animator.SetBool("WallSlide", false);
            _animator.SetBool("IdleBlock", false);
            _animator.SetFloat("AirSpeedY", 0f);
            _animator.SetInteger("AnimState", 0);

            if (_playerView != null)
                UpdatePlayerView();

            if (_deathMenu != null)
                _deathMenu.SetActive(false);
        }

        private void Update()
        {
            if (_isDead) return;

            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");

            _wasGrounded = _isGrounded;
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);

            HandleTimers();
            HandleGroundState();
            HandleDirection();
            HandleInput();
            HandleAnimations();
            HandleInvincibility();
        }

        private void FixedUpdate()
        {
            if (_isDead || _isRolling) return;

            float velocityY = _verticalMovementEnabled
                ? _verticalMovement * _climbSpeed
                : _rb.linearVelocity.y;

            _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, velocityY);
        }

        private void HandleTimers()
        {
            _timeSinceAttack += Time.deltaTime;

            if (_isRolling)
            {
                _rollCurrentTime += Time.deltaTime;
                if (_rollCurrentTime >= _rollDuration)
                {
                    _isRolling = false;
                    _rollCurrentTime = 0f;
                }
            }
        }

        private void HandleGroundState()
        {
            if (!_wasGrounded && _isGrounded) _animator.SetBool("Grounded", true);
            if (_wasGrounded && !_isGrounded) _animator.SetBool("Grounded", false);
        }

        private void HandleDirection()
        {
            if (_horizontalInput > 0) { _spriteRenderer.flipX = false; _facingDirection = 1; }
            else if (_horizontalInput < 0) { _spriteRenderer.flipX = true; _facingDirection = -1; }
        }

        private void HandleInput()
        {
            if (Input.GetButtonDown("Jump") && _isGrounded && !_isRolling) Jump();
            if (Input.GetMouseButtonDown(0) && _timeSinceAttack > 0.25f && !_isRolling) Attack();
            if (Input.GetMouseButtonDown(1) && !_isRolling) { _animator.SetTrigger("Block"); _animator.SetBool("IdleBlock", true); }
            if (Input.GetMouseButtonUp(1)) _animator.SetBool("IdleBlock", false);
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRolling && !_isWallSliding) Roll();
        }

        private void HandleAnimations()
        {
            _isWallSliding = (CheckWall(_wallCheckRight1) && CheckWall(_wallCheckRight2)) || (CheckWall(_wallCheckLeft1) && CheckWall(_wallCheckLeft2));
            _animator.SetBool("WallSlide", _isWallSliding);
            _animator.SetFloat("AirSpeedY", _rb.linearVelocity.y);

            if (_isRolling) return;

            if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon) { _delayToIdle = 0.05f; _animator.SetInteger("AnimState", 1); }
            else { _delayToIdle -= Time.deltaTime; if (_delayToIdle <= 0f) _animator.SetInteger("AnimState", 0); }
        }

        private void HandleInvincibility()
        {
            if (!_isInvincible) return;
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f) _isInvincible = false;
        }

        private bool CheckWall(Transform point) => point != null && Physics2D.OverlapCircle(point.position, _wallCheckRadius, _groundLayer);

        public void EnableVerticalMovement(bool enabled)
        {
            if (_isDead) return;
            _verticalMovementEnabled = enabled;
            _rb.bodyType = enabled ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            if (!enabled) _rb.simulated = true;
        }

        private void Jump()
        {
            _animator.SetTrigger("Jump");
            _animator.SetBool("Grounded", false);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        }

        private void Attack()
        {
            _currentAttack++;
            if (_currentAttack > 3 || _timeSinceAttack > 1f) _currentAttack = 1;

            _animator.SetTrigger("Attack" + _currentAttack);
            _timeSinceAttack = 0f;
            DealDamageToEnemies();
        }

        private void DealDamageToEnemies()
        {
            if (_attackPoint == null) return;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
            foreach (var enemy in enemies)
            {
                var damageable = enemy.GetComponent<IDamageable>();
                if (damageable != null) damageable.TakeDamage(_attackDamage);
            }
        }

        private void Roll()
        {
            _isRolling = true;
            _rollCurrentTime = 0f;
            _animator.SetTrigger("Roll");
            _rb.linearVelocity = new Vector2(_facingDirection * _rollForce, _rb.linearVelocity.y);
        }

        public void TakeDamage(int damage = 1)
        {
            if (_isInvincible || _currentHealth <= 0 || _isDead) return;

            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            UpdatePlayerView();

            PlayHurtAnimation();

            if (_currentHealth <= 0) Die();
            else { _isInvincible = true; _invincibilityTimer = _invincibilityDuration; }
        }

        public void Heal(int amount = 1)
        {
            if (_currentHealth <= 0 || _isDead) return;
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            UpdatePlayerView();
        }

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;

            _currentExperience += amount;
            while (_currentExperience >= _experienceToNextLevel)
            {
                _currentExperience -= _experienceToNextLevel;
                LevelUp();
            }
            UpdatePlayerView();
        }

        private void LevelUp()
        {
            _currentLevel++;
            _experienceToNextLevel += 25;

            _maxHealth++;
            _currentHealth = _maxHealth;
            _attackDamage++;

            UpdatePlayerView();
        }

        private void PlayHurtAnimation() { if (!_isDead) _animator.SetTrigger("Hurt"); }

        public void Die()
        {
            if (_isDead) return;
            _isDead = true;

            _horizontalInput = 0f;
            _verticalMovementEnabled = false;
            _isRolling = false;
            _isWallSliding = false;
            _isInvincible = false;

            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.simulated = true;
            _rb.linearVelocity = Vector2.zero;

            _animator.SetBool("IdleBlock", false);
            _animator.SetBool("WallSlide", false);
            _animator.SetInteger("AnimState", 0);
            _animator.SetTrigger("Death");

            if (_deathCoroutine != null) StopCoroutine(_deathCoroutine);
            _deathCoroutine = StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSecondsRealtime(_deathAnimationDuration);
            if (_deathMenu != null) _deathMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        /// <summary>
        /// ����� ������� HP, EXP �� ����� ���������
        /// </summary>
        private void UpdatePlayerView()
        {
            if (_playerView == null) return;

            float hpNormalized = (float)_currentHealth / _maxHealth;
            float expNormalized = (float)_currentExperience / _experienceToNextLevel;

            _playerView.UpdateAll(hpNormalized, expNormalized, _currentLevel);
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck != null) { Gizmos.color = _isGrounded ? Color.green : Color.red; Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius); }
            DrawWallCheck(_wallCheckRight1);
            DrawWallCheck(_wallCheckRight2);
            DrawWallCheck(_wallCheckLeft1);
            DrawWallCheck(_wallCheckLeft2);
            if (_attackPoint != null) { Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(_attackPoint.position, _attackRange); }
        }

        private void DrawWallCheck(Transform point) { if (point != null) { Gizmos.color = Color.cyan; Gizmos.DrawWireSphere(point.position, _wallCheckRadius); } }
    }
}