using UnityEngine;

namespace ChickenHunt
{
    public class HealthKit : MonoBehaviour, IShootable
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private float _killDistance = 15f;
        private Vector2 _direction;

        public void Initialize(Vector2 flyDirection)
        {
            _direction = flyDirection.normalized;
        }

        private void Update()
        {
            transform.Translate(_direction * _speed * Time.deltaTime);

            if (transform.position.magnitude > _killDistance)
            {
                Destroy(gameObject);
            }
        }

        public void OnShoot()
        {
            if (ChickensManager.Instance != null)
            {
                ChickensManager.Instance.AddLife();
            }
            Destroy(gameObject);
        }
    }
}