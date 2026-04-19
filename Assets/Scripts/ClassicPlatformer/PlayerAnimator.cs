using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        private Player _player;
        private Rigidbody2D _rb;

        private static readonly int AnimState = Animator.StringToHash("AnimState");
        private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int JumpTrig = Animator.StringToHash("Jump");
        private static readonly int HurtTrig = Animator.StringToHash("Hurt");
        private static readonly int DeathTrig = Animator.StringToHash("Death");

        private bool _wasGrounded = true;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponent<Player>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            bool isGrounded = _player.IsGrounded;

            _animator.SetBool(Grounded, isGrounded);
            _animator.SetFloat(AirSpeedY, _rb.linearVelocity.y);

            if (_wasGrounded && !isGrounded && _rb.linearVelocity.y > 0.1f)
                _animator.SetTrigger(JumpTrig);

            _wasGrounded = isGrounded;

            if (isGrounded)
            {
                bool isRunning = Mathf.Abs(_rb.linearVelocity.x) > 0.1f;
                _animator.SetInteger(AnimState, isRunning ? 1 : 0);
            }
        }

        public void PlayHurt()
        {
            _animator.SetTrigger(HurtTrig);
        }

        public void PlayDeath()
        {
            _animator.SetBool("noBlood", false);
            _animator.SetTrigger(DeathTrig);
        }
    }
}