using UnityEngine;
using RPG;
using Assets.FantasyMonsters.Common.Scripts;

public class AttackEnemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _currentHealth = 10;
    [SerializeField] private Transform _healthFill;

    [Header("Attack")]
    [SerializeField] private float _range = 1.5f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _cooldown = 1.5f;
    [SerializeField] private LayerMask _targetMask;

    private Monster _monster;
    private Player _player;
    private float _nextAttackTime;
    private Vector3 _healthFillStartScale;

    private void Awake()
    {
        _monster = GetComponent<Monster>();
        _player = FindAnyObjectByType<Player>();

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        if (_healthFill != null)
            _healthFillStartScale = _healthFill.localScale;

        UpdateHealthBar();
    }

    private void Update()
    {
        TryAttack();
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        UpdateHealthBar();
    }

    private void TryAttack()
    {
        if (Time.time < _nextAttackTime)
            return;

        Collider2D target = Physics2D.OverlapCircle(transform.position, _range, _targetMask);

        if (target == null)
            return;

        Player player = target.GetComponent<Player>();

        if (player == null)
            return;

        _nextAttackTime = Time.time + _cooldown;

        if (_monster != null)
            _monster.Attack();

        player.TakeDamage(_damageAmount);
    }

    private void Die()
    {
        FindAnyObjectByType<GameUIManager>()?.ShowEnemyDeath();
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        if (_healthFill == null)
            return;

        float percent = _maxHealth > 0 ? (float)_currentHealth / _maxHealth : 0f;
        percent = Mathf.Clamp01(percent);

        Vector3 newScale = _healthFillStartScale;
        newScale.x *= percent;
        _healthFill.localScale = newScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}