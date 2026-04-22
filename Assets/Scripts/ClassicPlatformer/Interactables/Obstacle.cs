using UnityEngine;

namespace ClassicPlatformer
{
    public class Obstacle : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private bool _instantKill = false;
        [SerializeField] private float _damageInterval = 1f; // кожну секунду

        private Player _player;
        private float _timer;

        private void Update()
        {
            if (_player == null) return;

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                DealDamage();
                _timer = _damageInterval;
            }
        }

        private void DealDamage()
        {
            if (_player == null) return;

            if (_instantKill)
                _player.TakeDamage(_player.MaxHealth);
            else
                _player.TakeDamage(_damage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _player = player;
                _timer = 0f; // щоб урон був одразу
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player) && player == _player)
            {
                _player = null;
            }
        }
    }
}