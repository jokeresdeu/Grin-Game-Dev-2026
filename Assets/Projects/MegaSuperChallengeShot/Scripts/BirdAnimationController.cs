using System.Collections;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class BirdAnimationController : MonoBehaviour
    {
        private static readonly int FlySpeedParam = Animator.StringToHash("FlySpeed");
        private static readonly int HitTriggerParam = Animator.StringToHash("Hit");
        private static readonly int DieTriggerParam = Animator.StringToHash("Die");
        private static readonly int IsDeadParam = Animator.StringToHash("IsDead");

        [SerializeField] private float _deathDelay = 0.6f;
        [SerializeField] private float _hitFlashDuration = 0.15f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _hitColor = new Color(1f, 0.4f, 0.4f, 1f);

        private Animator _animator;
        private BirdMover _mover;
        private Collider2D _collider;
        private Color _baseColor;
        private bool _isDead;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<BirdMover>();
            _collider = GetComponent<Collider2D>();

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (_spriteRenderer != null)
                _baseColor = _spriteRenderer.color;
        }

        private void Update()
        {
            if (_isDead || _mover == null)
                return;

            _animator.SetFloat(FlySpeedParam, _mover.CurrentSpeed);
        }

        public void PlayHit()
        {
            if (_isDead)
                return;

            _animator.SetTrigger(HitTriggerParam);
            StartCoroutine(HitFlash());
        }

        public void PlayDeathAndDestroy()
        {
            if (_isDead)
                return;

            _isDead = true;

            _animator.SetBool(IsDeadParam, true);
            _animator.SetTrigger(DieTriggerParam);

            if (_collider != null)
                _collider.enabled = false;

            if (_mover != null)
                _mover.StopMoving();

            Destroy(gameObject, _deathDelay);
        }

        private IEnumerator HitFlash()
        {
            if (_spriteRenderer == null)
                yield break;

            _spriteRenderer.color = _hitColor;
            yield return new WaitForSeconds(_hitFlashDuration);

            if (!_isDead && _spriteRenderer != null)
                _spriteRenderer.color = _baseColor;
        }
    }
}
