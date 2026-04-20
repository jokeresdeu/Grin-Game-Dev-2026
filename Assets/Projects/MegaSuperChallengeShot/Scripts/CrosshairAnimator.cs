using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CrosshairAnimator : MonoBehaviour
{
    [Header("Idle Pulse")]
    [SerializeField] private float _idlePulseSpeed = 2f;
    [SerializeField] private float _idlePulseAmount = 0.05f;

    [Header("Shoot Punch")]
    [SerializeField] private float _shootPunchScale = 1.4f;
    [SerializeField] private float _shootPunchDuration = 0.1f;
    [SerializeField] private Color _shootFlashColor = new Color(1f, 0.8f, 0f, 1f);

    [Header("Reload Spin")]
    [SerializeField] private float _reloadSpinDuration = 0.3f;
    [SerializeField] private float _reloadSpinDegrees = 360f;

    [Header("Empty Shake")]
    [SerializeField] private float _shakeIntensity = 0.1f;
    [SerializeField] private float _shakeDuration = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Vector3 _baseScale;
    private Coroutine _currentAnimation;
    private bool _isAnimating;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _baseScale = transform.localScale;
    }

    private void Update()
    {
        if (!_isAnimating)
        {
            float pulse = 1f + Mathf.Sin(Time.time * _idlePulseSpeed) * _idlePulseAmount;
            transform.localScale = _baseScale * pulse;
        }
    }

    public void PlayShootAnimation()
    {
        if (_currentAnimation != null) StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(ShootPunchCoroutine());
    }

    public void PlayReloadAnimation()
    {
        if (_currentAnimation != null) StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(ReloadSpinCoroutine());
    }

    public void PlayEmptyAnimation()
    {
        if (_currentAnimation != null) StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(EmptyShakeCoroutine());
    }

    private IEnumerator ShootPunchCoroutine()
    {
        _isAnimating = true;
        float half = _shootPunchDuration / 2f;
        float elapsed = 0f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            transform.localScale = _baseScale * Mathf.Lerp(1f, _shootPunchScale, t);
            _spriteRenderer.color = Color.Lerp(_originalColor, _shootFlashColor, t);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            transform.localScale = _baseScale * Mathf.Lerp(_shootPunchScale, 1f, t);
            _spriteRenderer.color = Color.Lerp(_shootFlashColor, _originalColor, t);
            yield return null;
        }

        transform.localScale = _baseScale;
        _spriteRenderer.color = _originalColor;
        _isAnimating = false;
        _currentAnimation = null;
    }

    private IEnumerator ReloadSpinCoroutine()
    {
        _isAnimating = true;
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsed < _reloadSpinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _reloadSpinDuration;
            float angle = Mathf.Lerp(0f, _reloadSpinDegrees, t);
            transform.rotation = startRotation * Quaternion.Euler(0f, 0f, -angle);
            yield return null;
        }

        transform.rotation = startRotation;
        _isAnimating = false;
        _currentAnimation = null;
    }

    private IEnumerator EmptyShakeCoroutine()
    {
        _isAnimating = true;
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-_shakeIntensity, _shakeIntensity);
            float offsetY = Random.Range(-_shakeIntensity, _shakeIntensity);
            transform.localScale = _baseScale + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }

        transform.localScale = _baseScale;
        _isAnimating = false;
        _currentAnimation = null;
    }
}
