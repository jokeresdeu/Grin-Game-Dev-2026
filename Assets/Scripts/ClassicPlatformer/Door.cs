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
        private bool _ready;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            // Затримка щоб тригер не спрацював одразу при старті
            Invoke(nameof(Enable), 0.5f);
        }

        private void Enable() => _ready = true;

        private void Update()
        {
            if (!_ready || _isOpen || _useTrigger) return;

            Collider2D hit = Physics2D.OverlapCircle(transform.position, _openRadius, _playerLayer);
            if (hit != null)
                Open();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_ready || !_useTrigger || _isOpen) return;
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
