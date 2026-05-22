using ClassicPlatformer;
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

        private int maxHealth = 100;
        private int maxMana = 100;
        private int _health;
        private int _mana;

        public int CurrentHealth => _health;
        public int MaxHealth => maxHealth;
        public int CurrentMana => _mana;
        public int MaxMana => maxMana;

        private Camera _camera;
        private Vector2 _moveInput;

        private void Awake()
        {
            _health = maxHealth;
            _mana = maxMana;
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleKeyboardInput();
            HandleClickInput();
            HandleInteraction();
            UpdateMovement();
        }

        private void HandleKeyboardInput()
        {
            _moveInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (Input.GetKeyDown(KeyCode.R))
                TakeDamage();

            if (Input.GetKeyDown(KeyCode.T))
                UseMana();
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
                    }
                }
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
            if (_moveInput.sqrMagnitude <= 0)
                return;

            Vector2 movement = _moveInput.normalized;

            if (CanMove(movement))
                Move(movement);
        }

        private void Move(Vector2 direction)
        {
            transform.position += (Vector3)direction * _moveSpeed * Time.deltaTime;
            if (direction.x != 0)
            {
                _playerTransform.localScale = new Vector3(
                    direction.x > 0 ? 1 : -1,
                    1,
                    1
                );
            }
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

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactRange);
        }

        private void UseMana()
        {
            _mana -= 10;
            _mana = Mathf.Max(_mana, 0);
        }

        private void TakeDamage()
        {
            _health -= 10;
            _health = Mathf.Max(_health, 0);
        }
    }
}