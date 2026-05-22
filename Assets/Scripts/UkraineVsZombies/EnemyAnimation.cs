using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
    }

    public void PlayHurt()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
    }

    public void PlayDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }
}