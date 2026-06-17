using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.16f;
        [SerializeField] private float _maxDistanceFromCenter = 16f;

        private Vector2 _direction;
        private float _speed;
        private int _damage;
        private float _life;
        private float _age;
        private bool _active;

        public void Activate(Vector3 position, Vector2 direction, float speed, int damage, float life)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            _direction = direction.normalized;
            _speed = speed;
            _damage = damage;
            _life = life;
            _age = 0f;
            _active = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_active)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            float dt = Time.deltaTime;
            transform.position += (Vector3)(_direction * (_speed * dt));
            _age += dt;

            if (_age >= _life || transform.position.sqrMagnitude > _maxDistanceFromCenter * _maxDistanceFromCenter)
            {
                Deactivate();
                return;
            }

            Enemy hit = FindHit();
            if (hit != null)
            {
                hit.TakeDamage(_damage);
                Deactivate();
            }
        }

        private Enemy FindHit()
        {
            Vector3 position = transform.position;
            var enemies = EnemyRegistry.Active;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (enemy == null || enemy.IsDead)
                    continue;

                float reach = enemy.Radius + _radius;
                if ((enemy.Position - position).sqrMagnitude <= reach * reach)
                    return enemy;
            }

            return null;
        }

        private void Deactivate()
        {
            _active = false;
            if (BulletPool.Instance != null)
                BulletPool.Instance.Return(this);
            else
                gameObject.SetActive(false);
        }
    }
}
