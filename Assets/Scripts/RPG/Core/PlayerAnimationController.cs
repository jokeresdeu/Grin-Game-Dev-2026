using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace RPG
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int StateHash = Animator.StringToHash("State");

        [Header("References")]
        [SerializeField] private Animator _animator;

        [Header("Layer Names")]
        [SerializeField] private string _upperLayerName = "Upper";

        [Header("State Names")]
        [SerializeField] private string _attackUpperState = "SlashMelee1H";

        private int _upperLayerIndex;

        private bool _isDead;
        private bool _isAttacking;
        private bool _isMoving;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            _upperLayerIndex = _animator.GetLayerIndex(_upperLayerName);
        }

        public void SetMovement(bool isMoving)
        {
            if (_isDead || _isMoving == isMoving)
                return;

            _isMoving = isMoving;
            _animator.SetInteger(StateHash, isMoving ? (int)CharacterState.Run : (int)CharacterState.Idle);
        }

        public void PlayAttack()
        {
            if (_isDead) return;

            _isAttacking = true;
            PlayStateOnce(_attackUpperState, _upperLayerIndex);
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
            _isMoving = false;
            _animator.SetInteger(StateHash, (int)CharacterState.DeathB);
        }

        private void PlayStateOnce(string stateName, int layerIndex)
        {
            if (_animator == null || layerIndex < 0) return;

            if (_animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName)
                && _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1f)
                return;

            _animator.Play(stateName, layerIndex, 0f);
        }

        public bool IsDead => _isDead;
        public bool IsAttacking => _isAttacking;
    }
}
