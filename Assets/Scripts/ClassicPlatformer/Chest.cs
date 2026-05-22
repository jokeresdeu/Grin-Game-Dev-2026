using UnityEngine;

namespace ClassicPlatformer
{
    // Скриня: Collider2D IsTrigger = true
    // Animator: bool isOpen (Closed → Open)
    [RequireComponent(typeof(Animator))]
    public class Chest : MonoBehaviour
    {
        [SerializeField] private int _coinsReward = 3;

        private Animator _animator;
        private bool _isOpen;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOpen) return;
            if (!other.TryGetComponent(out Player _)) return;

            Open();
        }

        private void Open()
        {
            _isOpen = true;
            _animator.SetBool("isOpen", true);
            GameManager.Instance?.AddScore(_coinsReward);
        }
    }
}
