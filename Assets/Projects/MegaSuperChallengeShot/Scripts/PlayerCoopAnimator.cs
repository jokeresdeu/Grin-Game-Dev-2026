using System.Collections;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class PlayerCoopAnimator : MonoBehaviour
    {
        private static readonly int DamageTriggerParam = Animator.StringToHash("Damage");
        private static readonly int IsAliveParam = Animator.StringToHash("IsAlive");
        private static readonly int DieTriggerParam = Animator.StringToHash("Die");

        [SerializeField] private float _shakeDuration = 0.25f;
        [SerializeField] private float _shakeAmplitude = 0.12f;

        private Animator _animator;
        private Vector3 _restPosition;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _restPosition = transform.localPosition;
            _animator.SetBool(IsAliveParam, true);
        }

        public void PlayDamage()
        {
            _animator.SetTrigger(DamageTriggerParam);
            StopAllCoroutines();
            StartCoroutine(ShakeRoutine());
        }

        public void PlayDeath()
        {
            _animator.SetBool(IsAliveParam, false);
            _animator.SetTrigger(DieTriggerParam);
        }

        private IEnumerator ShakeRoutine()
        {
            float elapsed = 0f;
            while (elapsed < _shakeDuration)
            {
                float decay = 1f - (elapsed / _shakeDuration);
                Vector2 offset = Random.insideUnitCircle * _shakeAmplitude * decay;
                transform.localPosition = _restPosition + new Vector3(offset.x, offset.y, 0f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = _restPosition;
        }
    }
}
