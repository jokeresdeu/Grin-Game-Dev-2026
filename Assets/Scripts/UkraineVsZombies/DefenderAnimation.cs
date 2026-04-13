using UnityEngine;

namespace UkraineVsZombies
{
    public class DefenderAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAttacking(bool value)
        {
            if (_animator != null)
                _animator.SetBool("isAttacking", value);
        }

        public void PlayDeath()
        {
            if (_animator != null)
                _animator.SetTrigger("Die");
        }
    }
}