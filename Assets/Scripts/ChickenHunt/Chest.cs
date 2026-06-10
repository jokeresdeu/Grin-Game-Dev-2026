using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 3f;

        [Header("Visual")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider2D;

        private Vector2 _flyDirection;
        private ChickensManager _manager;
        private bool _isOpened;

        public void Initialize(Vector2 flyDirection, ChickensManager manager)
        {
            _flyDirection = flyDirection.normalized;
            _manager = manager;
            _isOpened = false;

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = _flyDirection.x < 0;

            if (_collider2D != null)
                _collider2D.enabled = true;
        }

        private void Update()
        {
            if (_isOpened) return;

            transform.Translate(_flyDirection * _speed * Time.deltaTime, Space.World);
        }

        public void OnShoot(int damage)
        {
            if (_isOpened) return;

            _isOpened = true;

            if (_collider2D != null)
                _collider2D.enabled = false;

            if (_animator != null)
                _animator.SetTrigger("Open");

            if (_manager != null)
                _manager.OnChestOpened(this);

            Destroy(gameObject);
        }
    }
}