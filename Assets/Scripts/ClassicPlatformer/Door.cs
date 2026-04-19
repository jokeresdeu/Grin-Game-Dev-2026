using UnityEngine;

namespace ClassicPlatformer
{
    // Двері: Closed → Opening → Open
    // Animator: bool isOpen
    // Гравець відкриває при наближенні через OverlapCircle або Trigger
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float _openRadius = 2f;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private bool _useTrigger = true;

        private Animator _animator;
        private bool _isOpen;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isOpen || _useTrigger) return;

            // OverlapCircle варіант — відкривається при наближенні
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _openRadius, _playerLayer);
            if (hit != null)
                Open();
        }

        // Trigger варіант
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_useTrigger || _isOpen) return;
            if (!other.TryGetComponent(out PlayerHealth _)) return;
            Open();
        }

        private void Open()
        {
            _isOpen = true;
            _animator.SetBool("isOpen", true);
        }

        private void OnDrawGizmosSelected()
        {
            if (!_useTrigger)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _openRadius);
            }
        }
    }
}
