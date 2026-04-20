using UnityEngine;

public class HumanCompositeController : MonoBehaviour
{
    public enum HumanState
    {
        Idle,
        Walk,
        Jump,
        Crouch
    }

    [Header("Body Part References (drag from child objects)")]
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _bodyUpper;
    [SerializeField] private Transform _bodyLower;
    [SerializeField] private Transform _leftArm;
    [SerializeField] private Transform _rightArm;
    [SerializeField] private Transform _legs;
    [SerializeField] private Transform _sword;

    [Header("Walk Animation")]
    [SerializeField] private float _walkBobAmount = 0.05f;
    [SerializeField] private float _walkBobSpeed = 8f;
    [SerializeField] private float _walkArmSwing = 15f;
    [SerializeField] private float _walkLegSwing = 20f;

    [Header("Jump Animation")]
    [SerializeField] private float _jumpSquashX = 1.2f;
    [SerializeField] private float _jumpStretchY = 0.8f;
    [SerializeField] private float _jumpArmAngle = -30f;

    [Header("Crouch Animation")]
    [SerializeField] private float _crouchCompressY = 0.6f;
    [SerializeField] private float _crouchSpeed = 8f;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;

    private Vector3 _headOrigPos;
    private Vector3 _bodyUpperOrigPos;
    private Vector3 _bodyLowerOrigPos;
    private Vector3 _leftArmOrigPos;
    private Vector3 _rightArmOrigPos;
    private Vector3 _legsOrigPos;
    private Vector3 _swordOrigPos;

    private HumanState _currentState = HumanState.Idle;
    private bool _facingRight = true;
    private float _crouchLerp = 0f;

    private void Start()
    {
        if (_head != null) _headOrigPos = _head.localPosition;
        if (_bodyUpper != null) _bodyUpperOrigPos = _bodyUpper.localPosition;
        if (_bodyLower != null) _bodyLowerOrigPos = _bodyLower.localPosition;
        if (_leftArm != null) _leftArmOrigPos = _leftArm.localPosition;
        if (_rightArm != null) _rightArmOrigPos = _rightArm.localPosition;
        if (_legs != null) _legsOrigPos = _legs.localPosition;
        if (_sword != null) _swordOrigPos = _sword.localPosition;
    }

    private void Update()
    {
        HandleInput();
        DetermineState();

        switch (_currentState)
        {
            case HumanState.Walk:
                AnimateWalk();
                break;
            case HumanState.Jump:
                AnimateJump();
                break;
            case HumanState.Crouch:
                AnimateCrouch();
                break;
            default:
                AnimateIdle();
                break;
        }
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * (horizontal * _moveSpeed * Time.deltaTime);

        if (horizontal > 0.01f && !_facingRight) Flip();
        else if (horizontal < -0.01f && _facingRight) Flip();
    }

    private void DetermineState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        bool crouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool jumping = Input.GetKey(KeyCode.Space);

        HumanState newState;

        if (jumping)
            newState = HumanState.Jump;
        else if (crouching)
            newState = HumanState.Crouch;
        else if (Mathf.Abs(horizontal) > 0.01f)
            newState = HumanState.Walk;
        else
            newState = HumanState.Idle;

        if (newState != _currentState)
        {
            _currentState = newState;

            if (newState != HumanState.Crouch)
            {
                _crouchLerp = 0f;
                ResetParts();
            }
        }
    }

    private void AnimateIdle()
    {
        float bob = Mathf.Sin(Time.time * 2f) * 0.02f;
        if (_head != null) _head.localPosition = _headOrigPos + new Vector3(0f, bob, 0f);
        if (_bodyUpper != null) _bodyUpper.localPosition = _bodyUpperOrigPos + new Vector3(0f, bob * 0.5f, 0f);
    }

    private void AnimateWalk()
    {
        float t = Time.time * _walkBobSpeed;

        float bob = Mathf.Sin(t) * _walkBobAmount;
        if (_head != null) _head.localPosition = _headOrigPos + new Vector3(0f, bob, 0f);
        if (_bodyUpper != null) _bodyUpper.localPosition = _bodyUpperOrigPos + new Vector3(0f, bob * 0.7f, 0f);

        float armAngle = Mathf.Sin(t) * _walkArmSwing;
        if (_leftArm != null) _leftArm.localRotation = Quaternion.Euler(0f, 0f, armAngle);
        if (_rightArm != null) _rightArm.localRotation = Quaternion.Euler(0f, 0f, -armAngle);
        if (_sword != null) _sword.localRotation = Quaternion.Euler(0f, 0f, -armAngle);

        float legAngle = Mathf.Sin(t) * _walkLegSwing;
        if (_legs != null) _legs.localRotation = Quaternion.Euler(0f, 0f, legAngle * 0.5f);
    }

    private void AnimateJump()
    {
        float scaleX = Mathf.Lerp(1f, _jumpSquashX, Mathf.Abs(Mathf.Sin(Time.time * 4f)));
        float scaleY = Mathf.Lerp(1f, _jumpStretchY, Mathf.Abs(Mathf.Sin(Time.time * 4f)));

        if (_bodyUpper != null) _bodyUpper.localScale = new Vector3(scaleX, scaleY, 1f);
        if (_bodyLower != null) _bodyLower.localScale = new Vector3(scaleX, scaleY, 1f);

        if (_leftArm != null) _leftArm.localRotation = Quaternion.Euler(0f, 0f, -_jumpArmAngle);
        if (_rightArm != null) _rightArm.localRotation = Quaternion.Euler(0f, 0f, _jumpArmAngle);

        if (_head != null) _head.localPosition = _headOrigPos + new Vector3(0f, 0.1f, 0f);
    }

    private void AnimateCrouch()
    {
        _crouchLerp = Mathf.MoveTowards(_crouchLerp, 1f, Time.deltaTime * _crouchSpeed);

        if (_head != null)
            _head.localPosition = Vector3.Lerp(_headOrigPos, _headOrigPos + new Vector3(0f, -0.3f, 0f), _crouchLerp);

        if (_bodyUpper != null)
            _bodyUpper.localPosition = Vector3.Lerp(_bodyUpperOrigPos, _bodyUpperOrigPos + new Vector3(0f, -0.15f, 0f), _crouchLerp);

        float compressY = Mathf.Lerp(1f, _crouchCompressY, _crouchLerp);
        if (_bodyUpper != null)
            _bodyUpper.localScale = new Vector3(1f, compressY, 1f);

        if (_leftArm != null)
            _leftArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 20f, _crouchLerp));
        if (_rightArm != null)
            _rightArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, -20f, _crouchLerp));
    }

    private void ResetParts()
    {
        if (_head != null) { _head.localPosition = _headOrigPos; }
        if (_bodyUpper != null) { _bodyUpper.localPosition = _bodyUpperOrigPos; _bodyUpper.localScale = Vector3.one; _bodyUpper.localRotation = Quaternion.identity; }
        if (_bodyLower != null) { _bodyLower.localPosition = _bodyLowerOrigPos; _bodyLower.localScale = Vector3.one; }
        if (_leftArm != null) { _leftArm.localPosition = _leftArmOrigPos; _leftArm.localRotation = Quaternion.identity; }
        if (_rightArm != null) { _rightArm.localPosition = _rightArmOrigPos; _rightArm.localRotation = Quaternion.identity; }
        if (_legs != null) { _legs.localPosition = _legsOrigPos; _legs.localRotation = Quaternion.identity; }
        if (_sword != null) { _sword.localPosition = _swordOrigPos; _sword.localRotation = Quaternion.identity; }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}
