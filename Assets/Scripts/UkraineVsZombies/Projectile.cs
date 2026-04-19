using UnityEngine;

namespace UkraineVsZombies
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        private Enemy _target;
        private float _damage;

        public void Initialize(Enemy target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private void Update()
        {
            if (_target == null || !_target.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.transform.position) < 0.2f)
            {
                _target.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}