using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class CrosshairAnimator : MonoBehaviour
    {
        private static readonly int ShootTriggerParam = Animator.StringToHash("Shoot");
        private static readonly int ReloadTriggerParam = Animator.StringToHash("Reload");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayShoot()
        {
            _animator.SetTrigger(ShootTriggerParam);
        }

        public void PlayReload()
        {
            _animator.SetTrigger(ReloadTriggerParam);
        }
    }
}
