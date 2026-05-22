using Assets.FantasyMonsters.Common.Scripts;
using ClassicPlatformer;
using Platformer;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private Transform _healthFill;

    [Header("Reward")]
    [SerializeField] private int _expReward = 10;

    private int _currentHealth;
    private bool _isDead;

    private Monster _monster;
    private Collider2D _collider;
    private EnemyAttack _enemyAttack;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _monster = GetComponent<Monster>();
        _collider = GetComponent<Collider2D>();
        _enemyAttack = GetComponent<EnemyAttack>();

        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        if (_monster != null)
            _monster.Spring();

        UpdateBar();

        if (_currentHealth <= 0)
            Die();
    }

    private void UpdateBar()
    {
        if (_healthFill == null) return;

        float percent = Mathf.Clamp01((float)_currentHealth / _maxHealth);
        _healthFill.localScale = new Vector3(percent * 1.50f, 0.59f, 0f);
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;

        if (_collider != null)
            _collider.enabled = false;

        if (_enemyAttack != null)
            _enemyAttack.enabled = false;

        if (_monster != null)
            _monster.Die();

        // Äàºìî EXP ãðàâöþ
        Player player = FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.AddExperience(_expReward);
        }

        Destroy(gameObject, 1.5f);
    }
}