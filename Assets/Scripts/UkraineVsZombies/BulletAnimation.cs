using UnityEngine;

public class BulletAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayImpact()
    {
        if (_animator != null)
            _animator.SetTrigger("Impact");
    }
}