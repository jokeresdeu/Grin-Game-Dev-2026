using System.Collections;
using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _stopDistance = 0.1f;

        [Header("Obstacle Detection")]
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private float _raycastDistance = 0.5f;

        [Header("Interaction")]
        [SerializeField] private float _interactRange = 4f;
        [SerializeField] private LayerMask _npcLayer;

        [Header("Visual")]
        [SerializeField] private Transform _playerTransform;

        [Header("Health")]
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private Transform _healthFill;

        [Header("Death")]
        [SerializeField] private float _deathDelay = 1.5f;

        private int _health;

        private Camera _camera;
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private Vector3 _healthStartScale;

        private bool _isDead;

        private void Awake()
        {
            _camera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();

            _health = _maxHealth;

            if (_healthFill != null)
                _healthStartScale = _healthFill.localScale;

            UpdateHealthBar();
        }

        private void Update()
        {
            if (_isDead) return;

            HandleKeyboardInput();
            HandleAttack();
            HandleInteraction();
        }

        private void FixedUpdate()
        {
            if (_isDead) return;

            UpdateMovement();
        }

        private void HandleAttack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<PlayerAnimationController>()?.Attack();
            }
        }

        public void TakeDamage(int damage)
        {
            if (_isDead) return;

            _health -= damage;
            if (_health < 0) _health = 0;

            UpdateHealthBar();

            if (_health <= 0)
            {
                _isDead = true;
                float deathDelay = _deathDelay;

                FindAnyObjectByType<GameUIManager>()?.ShowPlayerDeath();

                PlayerAnimationController animationController = GetComponent<PlayerAnimationController>();
                animationController?.Die();

                if (animationController != null)
                    deathDelay = Mathf.Max(_deathDelay, animationController.GetDeathDuration());

                _moveInput = Vector2.zero;
                _rb.linearVelocity = Vector2.zero;
                _rb.simulated = false;

                StartCoroutine(DeathRoutine(deathDelay));
            }
        }

        private IEnumerator DeathRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        public void Heal(int amount)
        {
            if (_isDead) return;

            _health += amount;
            if (_health > _maxHealth) _health = _maxHealth;

            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (_healthFill == null) return;

            float percent = Mathf.Clamp01((float)_health / _maxHealth);

            Vector3 scale = _healthStartScale;
            scale.x *= percent;

            _healthFill.localScale = scale;
        }

        private void HandleKeyboardInput()
        {
            _moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        private void HandleInteraction()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TryInteractWithNearbyNPC();
        }

        private void TryInteractWithNearbyNPC()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _interactRange, _npcLayer);

            float closestDist = float.MaxValue;
            NPC closestNPC = null;

            foreach (Collider2D hit in hits)
            {
                NPC npc = hit.GetComponent<NPC>();
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
            Vector2 movement = _moveInput.normalized;
            Move(movement);
        }

        private void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude > 0 && !CanMove(direction))
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            _rb.linearVelocity = direction * _moveSpeed;

            if (direction.x > 0)
                _playerTransform.localScale = new Vector3(1f, 1f, 1f);
            else if (direction.x < 0)
                _playerTransform.localScale = new Vector3(-1f, 1f, 1f);
        }

        private bool CanMove(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rb.position, direction, _raycastDistance, _obstacleLayer);
            return hit.collider == null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _raycastDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactRange);
        }
    }
}
