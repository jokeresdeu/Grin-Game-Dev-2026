using UnityEngine;
using RPG;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackDuration = 0.5f;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationController _animationController;

    private bool _isAttacking;

    private void Awake()
    {
        if (_animationController == null)
            _animationController = GetComponentInChildren<PlayerAnimationController>();
    }

    public bool TryAttack()
    {
        if (_isAttacking)
            return false;

        StartCoroutine(AttackRoutine());
        return true;
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        if (_animationController != null)
            _animationController.PlayAttack();

        Attack();

        yield return new WaitForSeconds(_attackDuration);

        if (_animationController != null)
            _animationController.EndAttack();

        _isAttacking = false;
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            _attackRange,
            _enemyLayer
        );

        foreach (var hit in hits)
        {
            hit.SendMessage("TakeDamage", _damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
