using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class GameOverPanelAnimator : MonoBehaviour
    {
        private static readonly int ShowTriggerParam = Animator.StringToHash("Show");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayShow()
        {
            _animator.SetTrigger(ShowTriggerParam);
        }
    }
}
