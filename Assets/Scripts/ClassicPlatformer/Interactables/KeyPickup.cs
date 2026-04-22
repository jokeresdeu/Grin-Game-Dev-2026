using System.Collections;
using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Collider2D))]
    public class KeyPickup : MonoBehaviour
    {
        [Header("Key Settings")]
        [SerializeField] private int _keyValue = 1;
        [SerializeField] private float _pickupDelay = 0.4f;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _pickupTriggerName = "Pickup";
        [SerializeField] private bool _disableColliderOnPickup = true;
        [SerializeField] private bool _hideSpriteOnPickup = false;

        private bool _isCollected;
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_collider != null)
                _collider.isTrigger = true;

            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCollected) return;

            Player player = other.GetComponent<Player>();
            if (player == null) return;

            KeyCollector keyCollector = player.GetComponent<KeyCollector>();
            if (keyCollector == null)
            {
                Debug.LogWarning("Player does not have KeyCollector!");
                return;
            }

            CollectKey(keyCollector);
        }

        private void CollectKey(KeyCollector keyCollector)
        {
            _isCollected = true;

            keyCollector.AddKey(_keyValue);

            if (_disableColliderOnPickup && _collider != null)
                _collider.enabled = false;

            if (_animator != null)
            {
                _animator.SetTrigger(_pickupTriggerName);
            }
            else
            {
                if (_hideSpriteOnPickup && _spriteRenderer != null)
                    _spriteRenderer.enabled = false;
            }

            StartCoroutine(DestroyAfterPickup());
        }

        private IEnumerator DestroyAfterPickup()
        {
            yield return new WaitForSeconds(_pickupDelay);
            Destroy(gameObject);
        }
    }
}