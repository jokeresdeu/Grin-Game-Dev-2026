using UnityEngine;

namespace RPG
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _stopDistance = 0.1f;

        [Header("Obstacle Detection")]
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private float _raycastDistance = 0.5f;

        [Header("Interaction")]
        [SerializeField] private float _interactRange = 2f;
        [SerializeField] private LayerMask _npcLayer;

        [Header("Visual")]
        [SerializeField] private Transform _playerTransform;

        private Camera _camera;
        private Vector2 _moveInput;
        private Animator _animator;
        private bool _isDead = false;

        [Header("Combat")]
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _attackDamage = 25f;
        private float _attackCooldownTimer = 0f;

        [Header("Health")]
        [SerializeField] private float _maxHealth = 100f;
        private float _currentHealth;

        private void Awake()
        {
            _camera = Camera.main;
            if (_maxHealth <= 0) _maxHealth = 100f;
            _currentHealth = _maxHealth;
            _animator = GetComponentInChildren<Animator>();
            
            if (_animator != null && _animator.gameObject.GetComponent<AnimationEventReceiver>() == null)
            {
                _animator.gameObject.AddComponent<AnimationEventReceiver>();
            }
        }

        private void Start()
        {
            if (RPGGameManager.Instance != null)
                RPGGameManager.Instance.UpdateHealthUI(_currentHealth, _maxHealth);
                
            if (_animator != null)
                _animator.Play("Run");
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            _currentHealth -= amount;
            if (_currentHealth < 0) _currentHealth = 0;
            
            if (RPGGameManager.Instance != null)
                RPGGameManager.Instance.UpdateHealthUI(_currentHealth, _maxHealth);

            if (_animator != null && amount > 0 && _currentHealth > 0)
                _animator.SetTrigger("Hit");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            Debug.Log("PLAYER DIED!");
            _isDead = true;
            if (_animator != null)
            {
                _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                _animator.SetInteger("State", 6);
                _animator.SetBool("DieBack", true);
                _animator.Play("DeathBack", 1);
            }
                
            if (RPGGameManager.Instance != null)
                RPGGameManager.Instance.GameOver();
        }

        private void Update()
        {
            if (_isDead) return;
            if (RPGGameManager.Instance != null && RPGGameManager.Instance.IsGameOver)
            {
                if (_animator != null) _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                _moveInput = Vector2.zero;
                UpdateAnimations();
                return;
            }
            
            HandleKeyboardInput();
            HandleClickInput();
            HandleInteraction();
            UpdateMovement();

            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            if (_animator == null) return;

            if (_moveInput.sqrMagnitude > 0)
            {
                _animator.SetInteger("State", 3);
            }
            else
            {
                _animator.SetInteger("State", 0);
            }
        }

        private void HandleKeyboardInput()
        {
            _moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );
        }

        private void HandleClickInput()
        {
            _attackCooldownTimer -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && _attackCooldownTimer <= 0f)
            {
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            _attackCooldownTimer = 0.5f;
            if (_animator != null)
            {
                _animator.Play("SlashMelee1H", 0);
            }

            var hits = Physics2D.OverlapCircleAll(transform.position, _attackRange);
            foreach (var hit in hits)
            {
                Vector3 dirToHit = (hit.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.right, dirToHit) < 0f)
                {
                    continue;
                }

                var enemy = hit.GetComponent<RPGEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(_attackDamage);
                }

                var chest = hit.GetComponent<RPGChest>();
                if (chest != null)
                {
                    chest.TakeDamage(_attackDamage);
                }
            }
        }

        public void SetExpression(string id) {}

        public void CustomEvent(string id) {}

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
                movement = _moveInput.normalized;

            if (movement.sqrMagnitude > 0 && CanMove(movement))
                Move(movement);
        }

        private void Move(Vector2 direction)
        {
            Vector3 newPos = transform.position + (Vector3)direction * _moveSpeed * Time.deltaTime;
            newPos.y = Mathf.Clamp(newPos.y, -4.5f, -2.5f);

            if (_camera != null)
            {
                float camHeight = 2f * _camera.orthographicSize;
                float camWidth = camHeight * _camera.aspect;
                
                float minX = _camera.transform.position.x - (camWidth / 2f) + 0.5f;
                float maxX = _camera.transform.position.x + (camWidth / 2f) - 0.5f;
                
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            }

            transform.position = newPos;

            if (direction.x > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (direction.x < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        private bool CanMove(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _raycastDistance, _obstacleLayer);
            return hit.collider == null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _raycastDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}

