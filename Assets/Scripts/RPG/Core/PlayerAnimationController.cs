using UnityEngine;

namespace RPG
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _animator;

        [Header("Layer Names")]
        [SerializeField] private string _lowerLayerName = "Lower";
        [SerializeField] private string _upperLayerName = "Upper";

        [Header("State Names")]
        [SerializeField] private string _runState = "Run";
        [SerializeField] private string _deathState = "DeathBack";
        [SerializeField] private string _attackUpperState = "SlashMelee1H";

        private int _lowerLayerIndex;
        private int _upperLayerIndex;

        private bool _isDead;
        private bool _isAttacking;
        private bool _isMoving;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            _lowerLayerIndex = _animator.GetLayerIndex(_lowerLayerName);
            _upperLayerIndex = _animator.GetLayerIndex(_upperLayerName);
        }

        public void SetMovement(bool isMoving)
        {
            if (_isDead) return;

            _isMoving = isMoving;

            if (isMoving)
            {
                PlayState(_runState, _lowerLayerIndex);
            }
        }

        public void PlayAttack()
        {
            if (_isDead) return;

            _isAttacking = true;
            PlayState(_attackUpperState, _upperLayerIndex);
        }

        public void EndAttack()
        {
            if (_isDead) return;

            _isAttacking = false;
        }

        public void PlayDeath()
        {
            if (_isDead) return;

            _isDead = true;
            PlayState(_deathState, _lowerLayerIndex);
        }

        private void PlayState(string stateName, int layerIndex)
        {
            if (_animator == null) return;
            if (layerIndex < 0) return;

            _animator.Play(stateName, layerIndex, 0f);
        }

        public bool IsDead => _isDead;
        public bool IsAttacking => _isAttacking;
    }
}