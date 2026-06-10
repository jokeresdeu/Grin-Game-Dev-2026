using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _jumpVelocity = 11f;
        [SerializeField] private float _fallMultiplier = 2.4f;
        [SerializeField] private float _lowJumpMultiplier = 1.6f;
        [SerializeField] private float _coyoteSeconds = 0.08f;
        [SerializeField] private float _jumpBufferSeconds = 0.12f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.18f;
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private float _rotationSpeed = 540f;
        [SerializeField] private PlayerHealth _health;

        private Rigidbody2D _rigidbody;
        private float _coyoteTimer;
        private float _jumpBufferTimer;
        private bool _isGrounded;
        private bool _holdingJump;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_health == null)
                _health = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            if (!IsPlaying())
                return;

            UpdateTimers();
            ReadInput();
            HandleJump();
            RotateInAir();
        }

        private void FixedUpdate()
        {
            if (!IsPlaying())
                return;

            ApplyVariableGravity();
        }

        private bool IsPlaying()
        {
            return GameManager.Instance == null || GameManager.Instance.State == GameState.Playing;
        }

        private void UpdateTimers()
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayers);

            if (_isGrounded)
                _coyoteTimer = _coyoteSeconds;
            else
                _coyoteTimer = Mathf.Max(0f, _coyoteTimer - Time.deltaTime);

            _jumpBufferTimer = Mathf.Max(0f, _jumpBufferTimer - Time.deltaTime);
        }

        private void ReadInput()
        {
            bool pressed = Input.GetKeyDown(KeyCode.Space)
                           || Input.GetKeyDown(KeyCode.UpArrow)
                           || Input.GetKeyDown(KeyCode.W)
                           || Input.GetMouseButtonDown(0)
                           || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

            bool held = Input.GetKey(KeyCode.Space)
                        || Input.GetKey(KeyCode.UpArrow)
                        || Input.GetKey(KeyCode.W)
                        || Input.GetMouseButton(0)
                        || (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(0).phase != TouchPhase.Canceled);

            _holdingJump = held;

            if (pressed)
                _jumpBufferTimer = _jumpBufferSeconds;
        }

        private void HandleJump()
        {
            if (_jumpBufferTimer > 0f && _coyoteTimer > 0f)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpVelocity);
                _jumpBufferTimer = 0f;
                _coyoteTimer = 0f;
            }
        }

        private void RotateInAir()
        {
            if (!_isGrounded)
            {
                transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
            }
            else
            {
                float current = transform.eulerAngles.z;
                float snapped = Mathf.Round(current / 90f) * 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, snapped);
            }
        }

        private void ApplyVariableGravity()
        {
            Vector2 velocity = _rigidbody.linearVelocity;

            if (velocity.y < 0f)
                velocity += Vector2.up * (Physics2D.gravity.y * (_fallMultiplier - 1f) * Time.fixedDeltaTime);
            else if (velocity.y > 0f && !_holdingJump)
                velocity += Vector2.up * (Physics2D.gravity.y * (_lowJumpMultiplier - 1f) * Time.fixedDeltaTime);

            _rigidbody.linearVelocity = velocity;
        }

        private void OnDrawGizmosSelected()
        {
            if (_groundCheck == null)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
