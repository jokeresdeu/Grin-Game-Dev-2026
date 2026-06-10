using UnityEngine;

public class ChickenAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector3 lastPosition;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (animator == null)
            return;

        bool isMoving = (transform.position - lastPosition).sqrMagnitude > 0.00001f;
        animator.SetBool("IsMoving", isMoving);

        lastPosition = transform.position;
    }

    public void PlayHit()
    {
        if (animator != null)
            animator.SetTrigger("Hit");
    }

    public void PlayDeath()
    {
        if (animator != null)
            animator.SetTrigger("Death");
    }
}