using UnityEngine;

namespace ClassicPlatformer
{
    public class FireDamageZone : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _damageInterval = 1f;

        private float _timer;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player)) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                player.TakeDamage(_damage);
                _timer = _damageInterval;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player _))
                _timer = 0f;
        }
    }
}