using UnityEngine;
using RPG;
using Assets.FantasyMonsters.Common.Scripts;

public class PiggyAttack : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 77;
    [SerializeField] private int _currentHealth = 77;
    [SerializeField] private Transform _healthFill;

    [Header("Counter Attack")]
    [SerializeField] private int _damage = 77;
    [SerializeField] private float _range = 2f;

    private Monster _monster;
    private Player _player;
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

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        _currentHealth -= amount;

        if (_currentHealth < 0)
            _currentHealth = 0;

        UpdateHealthBar();
        CounterAttack();
    }

    private void CounterAttack()
    {
        if (_player == null)
            return;

        float distance = Vector2.Distance(transform.position, _player.transform.position);

        if (distance > _range)
            return;

        if (_monster != null)
            _monster.Attack();

        _player.TakeDamage(_damage);
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
}