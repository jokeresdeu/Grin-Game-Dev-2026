using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageable
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _stopDistance = 0.1f;

        [Header("Interaction")]
        [SerializeField] private float _interactRange = 4f;
        [SerializeField] private LayerMask _npcLayer;

        [Header("Visual")]
        [SerializeField] private Transform _playerTransform;

        [Header("Health")]
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private Transform _healthFill;

        [Header("Animation")]
        [SerializeField] private PlayerAnimationController _animationController;

        private int _health;

        private Camera _camera;
        private Rigidbody2D _rb;

        private Vector2 _moveInput;
        private Vector3 _clickDestination;
        private bool _isMovingToClick;
        private bool _isDead;

        private void Awake()
        {
            _camera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();

            if (_animationController == null)
                _animationController = GetComponentInChildren<PlayerAnimationController>();

            _health = _maxHealth;
            UpdateHealthBar();
        }

        private void Update()
        {
            if (_isDead) return;

            HandleKeyboardInput();
            HandleClickInput();
            HandleInteraction();
        }

        private void FixedUpdate()
        {
            if (_isDead) return;

            UpdateMovement();
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;

            _health -= damage;
            UpdateHealthBar();

            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _isDead = true;
            _rb.linearVelocity = Vector2.zero;

            if (_animationController != null)
                _animationController.PlayDeath();

            GameManager.Instance?.PlayerDied();

            // Даємо анімації смерті програтись
            Destroy(gameObject, 1.5f);
        }

        private void UpdateHealthBar()
        {
            if (_healthFill == null) return;

            float percent = Mathf.Clamp01((float)_health / _maxHealth);
            _healthFill.localScale = new Vector3(percent * 12.5f, 3f, 0f);
        }

        private void HandleKeyboardInput()
        {
            _moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (_moveInput.sqrMagnitude > 0)
            {
                _isMovingToClick = false;
            }
        }

        private void HandleClickInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;

                var hit = Physics2D.Raycast(mouseWorld, Vector2.zero, 0f, _npcLayer);
                if (hit.collider != null)
                {
                    var npc = hit.collider.GetComponent<NPC>();
                    if (npc != null && npc.IsInRange)
                    {
                        npc.Interact();
                        return;
                    }
                }

                _clickDestination = mouseWorld;
                _isMovingToClick = true;
            }
        }

        private void HandleInteraction()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TryInteractWithNearbyNPC();
        }

        private void TryInteractWithNearbyNPC()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, _interactRange, _npcLayer);

            float closestDist = float.MaxValue;
            NPC closestNPC = null;

            foreach (var hit in hits)
            {
                var npc = hit.GetComponent<NPC>();
                if (npc == null) continue;

                float dist = Vector2.Distance(transform.position, npc.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestNPC = npc;
                }
            }

            if (closestNPC != null)
                closestNPC.Interact();
        }

        private void UpdateMovement()
        {
            Vector2 movement = Vector2.zero;

            if (_moveInput.sqrMagnitude > 0)
            {
                movement = _moveInput.normalized;
            }
            else if (_isMovingToClick)
            {
                movement = GetClickMovement();
            }

            Move(movement);
        }

        private Vector2 GetClickMovement()
        {
            Vector3 direction = _clickDestination - transform.position;

            if (direction.magnitude <= _stopDistance)
            {
                _isMovingToClick = false;
                return Vector2.zero;
            }

            return ((Vector2)direction).normalized;
        }

        private void Move(Vector2 direction)
        {
            _rb.linearVelocity = direction * _moveSpeed;

            bool isMoving = direction.sqrMagnitude > 0.01f;

            if (_animationController != null)
                _animationController.SetMovement(isMoving);

            if (direction.x > 0)
                _playerTransform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                _playerTransform.localScale = new Vector3(-1, 1, 1);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactRange);

            if (_isMovingToClick)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, _clickDestination);
                Gizmos.DrawWireSphere(_clickDestination, 0.2f);
            }
        }
    }
}