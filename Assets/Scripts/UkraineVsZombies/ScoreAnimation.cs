using UnityEngine;

public class ScoreAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayPop()
    {
        if (_animator != null)
            _animator.SetTrigger("Pop");
    }
}