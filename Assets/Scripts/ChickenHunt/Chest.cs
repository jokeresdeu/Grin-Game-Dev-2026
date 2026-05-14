using System;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Points")]
        [SerializeField] private int _points = 250;

        [Header("HP")]
        [SerializeField] private int _maxHp = 1;

        private int _currentHp;

        public event Action<int> OnDeath;

        private void Start()
        {
            _currentHp = _maxHp;
        }

        public void OnShoot(int damage)
        {
            _currentHp -= damage;

            if (_currentHp <= 0)
            {
                OnDeath?.Invoke(_points);
                Destroy(gameObject);
            }
        }
    }
}