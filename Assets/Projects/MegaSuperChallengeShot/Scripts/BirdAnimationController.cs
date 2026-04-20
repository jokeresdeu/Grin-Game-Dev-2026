using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BirdAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _hitFlashDuration = 0.15f;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _deathFallSpeed = 3f;
    [SerializeField] private float _deathFadeDuration = 0.8f;
    [SerializeField] private float _deathRotationSpeed = 200f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isDead;
    private bool _isHit;

    private static readonly int IsHitHash = Animator.StringToHash("IsHit");
    private static readonly int IsDeadHash = Animator.StringToHash("IsDead");
    private static readonly int FlySpeedHash = Animator.StringToHash("FlySpeed");

    public bool IsDead => _isDead;

    public event Action OnDeathAnimationComplete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    public void SetFlySpeed(float speed)
    {
        if (_isDead) return;
        if (_animator != null)
            _animator.SetFloat(FlySpeedHash, Mathf.Abs(speed));
    }

    public void PlayHitEffect()
    {
        if (_isDead || _isHit) return;

        if (_animator != null)
            _animator.SetTrigger(IsHitHash);
        StartCoroutine(HitFlashCoroutine());
    }

    public void PlayDeathAnimation()
    {
        if (_isDead) return;

        _isDead = true;
        if (_animator != null)
            _animator.SetBool(IsDeadHash, true);
        StartCoroutine(DeathSequenceCoroutine());
    }

    private IEnumerator HitFlashCoroutine()
    {
        _isHit = true;
        _spriteRenderer.color = _hitColor;
        yield return new WaitForSeconds(_hitFlashDuration);
        _spriteRenderer.color = _originalColor;
        _isHit = false;
    }

    private IEnumerator DeathSequenceCoroutine()
    {
        _spriteRenderer.color = _hitColor;
        yield return new WaitForSeconds(_hitFlashDuration);

        float elapsed = 0f;
        Color startColor = _spriteRenderer.color;

        while (elapsed < _deathFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _deathFadeDuration;
            transform.position += Vector3.down * (_deathFallSpeed * Time.deltaTime);
            transform.Rotate(0f, 0f, _deathRotationSpeed * Time.deltaTime);
            Color c = startColor;
            c.a = Mathf.Lerp(1f, 0f, t);
            _spriteRenderer.color = c;

            yield return null;
        }

        OnDeathAnimationComplete?.Invoke();
        Destroy(gameObject);
    }
}
