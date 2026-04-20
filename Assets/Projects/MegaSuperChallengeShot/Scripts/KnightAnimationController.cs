using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class KnightAnimationController : MonoBehaviour
{
    public enum KnightState
    {
        Idle,
        Walk,
        Jump,
        Crouch
    }

    [Header("Idle Sprites (Knight_idle_01..06)")]
    [SerializeField] private Sprite[] _idleFrames;

    [Header("Walk Sprites (Knight_walk_01..06)")]
    [SerializeField] private Sprite[] _walkFrames;

    [Header("Jump Sprites (Knight_jump_01..02)")]
    [SerializeField] private Sprite[] _jumpFrames;

    [Header("Crouch Sprite (Knight_crouch_0)")]
    [SerializeField] private Sprite[] _crouchFrames;

    [Header("Animation Speed")]
    [SerializeField] private float _idleFrameRate = 6f;
    [SerializeField] private float _walkFrameRate = 10f;
    [SerializeField] private float _jumpFrameRate = 4f;
    [SerializeField] private float _crouchFrameRate = 4f;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;
    private KnightState _currentState = KnightState.Idle;
    private KnightState _previousState = KnightState.Idle;
    private float _frameTimer;
    private int _currentFrame;
    private bool _isGrounded;
    private bool _facingRight = true;

    public KnightState CurrentState => _currentState;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        CheckGrounded();
        DetermineState();
        AnimateCurrentState();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = new Vector2(horizontal * _moveSpeed, _rigidbody.linearVelocity.y);
        }
        else
        {
            transform.position += Vector3.right * (horizontal * _moveSpeed * Time.deltaTime);
        }

        if (horizontal > 0.01f && !_facingRight) Flip();
        else if (horizontal < -0.01f && _facingRight) Flip();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
            }
        }
    }

    private void CheckGrounded()
    {
        if (_groundCheck != null)
        {
            _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        }
        else
        {
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, _groundLayer);
        }
    }

    private void DetermineState()
    {
        _previousState = _currentState;

        float horizontal = Input.GetAxisRaw("Horizontal");
        bool crouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (!_isGrounded)
        {
            _currentState = KnightState.Jump;
        }
        else if (crouching)
        {
            _currentState = KnightState.Crouch;
        }
        else if (Mathf.Abs(horizontal) > 0.01f)
        {
            _currentState = KnightState.Walk;
        }
        else
        {
            _currentState = KnightState.Idle;
        }

        if (_currentState != _previousState)
        {
            _currentFrame = 0;
            _frameTimer = 0f;
        }
    }

    private void AnimateCurrentState()
    {
        Sprite[] frames;
        float frameRate;

        switch (_currentState)
        {
            case KnightState.Walk:
                frames = _walkFrames;
                frameRate = _walkFrameRate;
                break;
            case KnightState.Jump:
                frames = _jumpFrames;
                frameRate = _jumpFrameRate;
                break;
            case KnightState.Crouch:
                frames = _crouchFrames;
                frameRate = _crouchFrameRate;
                break;
            default:
                frames = _idleFrames;
                frameRate = _idleFrameRate;
                break;
        }

        if (frames == null || frames.Length == 0) return;

        _frameTimer += Time.deltaTime;
        float frameInterval = 1f / frameRate;

        if (_frameTimer >= frameInterval)
        {
            _frameTimer -= frameInterval;
            _currentFrame = (_currentFrame + 1) % frames.Length;
        }

        _spriteRenderer.sprite = frames[_currentFrame];
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
