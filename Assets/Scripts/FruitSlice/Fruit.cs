using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FruitSlice
{
    public class Fruit : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField] private int _points = 1;

        [Header("Halves (Children)")]
        [SerializeField] private GameObject _leftHalf;
        [SerializeField] private GameObject _rightHalf;

        [Header("Slice Physics")]
        [SerializeField] private float _separationForce = 3f;
        [SerializeField] private float _upwardForce = 2f;
        [SerializeField] private float _torque = 10f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _wholeSprite;

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private bool _isSliced;

        public event Action<int> OnSliced;
        public bool IsSliced => _isSliced;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public void Initialize(Vector2 launchDirection, float launchForce, float angularVelocity)
        {
            if (_leftHalf != null)
                _leftHalf.SetActive(false);

            if (_rightHalf != null)
                _rightHalf.SetActive(false);

            if (_rb != null)
            {
                _rb.linearVelocity = launchDirection * launchForce;
                _rb.angularVelocity = angularVelocity;
            }
        }

        public void Slice(Vector2 sliceDirection)
        {
            if (_isSliced) return;
            _isSliced = true;

            OnSliced?.Invoke(_points);

            if (_wholeSprite != null)
                _wholeSprite.enabled = false;

            if (_collider != null)
                _collider.enabled = false;

            if (_rb != null)
                _rb.simulated = false;

            Vector2 perpendicular = new Vector2(-sliceDirection.y, sliceDirection.x);

            ReleaseHalf(_leftHalf, -perpendicular);
            ReleaseHalf(_rightHalf, perpendicular);
        }

        private void ReleaseHalf(GameObject half, Vector2 direction)
        {
            if (half == null) return;

            half.SetActive(true);
            half.transform.SetParent(null);

            var rb = half.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = half.AddComponent<Rigidbody2D>();

            rb.linearVelocity = direction * _separationForce + Vector2.up * _upwardForce;
            rb.angularVelocity = Random.Range(-_torque, _torque) * 10f;
            rb.gravityScale = 1f;

            var halfScript = half.GetComponent<FruitHalf>();
            if (halfScript == null)
                half.AddComponent<FruitHalf>();
        }
    }
}
