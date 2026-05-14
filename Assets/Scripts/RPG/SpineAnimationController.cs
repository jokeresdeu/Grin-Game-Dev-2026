using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineAnimationController : MonoBehaviour
{
    [SpineAnimation, SerializeField] private string _runAnimation;
    [SpineAnimation, SerializeField] private string _idleAnimation;
    [SpineAnimation, SerializeField] private string _atackAnimation;
    
    [SerializeField] private SkeletonAnimation _animation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrackEntry trackEntry = _animation.AnimationState.SetAnimation(0, _atackAnimation, false);
            trackEntry.Complete += OnAnimationCompleted;
        }
    }

    private void OnAnimationCompleted(TrackEntry trackentry)
    {
        _animation.AnimationState.SetAnimation(0, _idleAnimation, true);
    }
}
