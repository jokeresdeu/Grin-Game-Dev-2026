using UnityEngine;

namespace ChickenHunt
{
    public class BossShield : MonoBehaviour, IShootable
    {
        [Header("Shield HP")]
        [SerializeField] private int _maxHP = 3;

        private int _currentHP;
        private BossChicken _boss;

        private void Awake()
        {
            _currentHP = _maxHP;
        }

        public void Initialize(BossChicken boss)
        {
            _boss = boss;
            _currentHP = _maxHP;
        }

        public void OnShoot()
        {
            _currentHP--;

            if (_currentHP <= 0)
            {
                if (_boss != null)
                    _boss.RemoveShield(this);

                Destroy(gameObject);
            }
        }
    }
}